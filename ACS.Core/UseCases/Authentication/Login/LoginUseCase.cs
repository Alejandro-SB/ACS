using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ACS.Core.Errors;
using ACS.Core.Interfaces;
using ACS.Core.Interfaces.Repositories;
using ACS.Core.Interfaces.UseCases;

namespace ACS.Core.UseCases.Authentication.Login
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginUseCase(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(LoginRequest message, IOutputPort<LoginResponse> outputPort)
        {
            var user = await _userRepository.FindByNameAsync(message.UserName);

            if(user != null && await _userRepository.CheckPasswordAsync(user, message.Password))
            {
                var refreshToken = _tokenService.GenerateToken();
                user.AddRefreshToken(refreshToken, user.Id, message.Ip);
                await _userRepository.UpdateAsync(user);

                outputPort.Handle(new LoginResponse(_tokenService.GenerateEncodedToken(user.IdentityId, user.UserName, refreshToken), true));
                return true;
            }

            outputPort.Handle(new LoginResponse(new[] { ACSErrors.Authentication.InvalidLogin }));
            return false;
        }
    }
}
