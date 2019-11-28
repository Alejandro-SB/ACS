using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ACS.Core.Interfaces;
using ACS.Core.Interfaces.Repositories;
using ACS.Core.Interfaces.UseCases;
using ACS.Core.QueryFilter;

namespace ACS.Core.UseCases.Authentication.RefreshToken
{
    public class ExchangeRefreshTokenUseCase : IExchangeRefreshTokenUseCase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public ExchangeRefreshTokenUseCase(ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(ExchangeRefreshTokenRequest message, IOutputPort<ExchangeRefreshTokenResponse> outputPort)
        {
            var principal = _tokenService.GetPrincipalFromToken(message.AccessToken, message.SigningKey);

            if (principal != null)
            {
                var id = principal.Claims.First(c => c.Type == "id");
                var identityId = int.Parse(id.Value);
                var user = await _userRepository.FirstOrDefaultByQueryAsync(new UserQueryFilter(identityId));

                if (user!.HasValidRefreshToken(message.RefreshToken))
                {
                    var refreshToken = _tokenService.GenerateToken();
                    var token = _tokenService.GenerateEncodedToken(user.IdentityId, user.UserName, refreshToken);
                    user.RemoveRefreshToken(message.RefreshToken);
                    user.AddRefreshToken(refreshToken, user.Id, "");
                    await _userRepository.UpdateAsync(user);
                    outputPort.Handle(new ExchangeRefreshTokenResponse(token, true));
                    return true;
                }
            }

            outputPort.Handle(new ExchangeRefreshTokenResponse(false, "Invalid token."));

            return false;
        }
    }
}