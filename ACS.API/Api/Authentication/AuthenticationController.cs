using ACS.API.Api.Authentication.Models;
using ACS.API.Api.Authentication.Presenters;
using ACS.API.Authentication;
using ACS.Core.UseCases.Authentication.Login;
using ACS.Core.UseCases.Authentication.RefreshToken;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ACS.API.Api.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoginUseCase _loginUseCase;
        private readonly IExchangeRefreshTokenUseCase _exchangeRefreshTokenUseCase;
        private readonly LoginPresenter _loginPresenter;
        private readonly ExchangeRefreshTokenPresenter _exchangeRefreshTokenPresenter;
        private readonly JwtOptions _jwtOptions;

        public AuthenticationController(IOptions<JwtOptions> jwtOptions,
            ILoginUseCase loginUseCase, LoginPresenter loginPresenter, 
            IExchangeRefreshTokenUseCase exchangeRefreshTokenUseCase, ExchangeRefreshTokenPresenter exchangeRefreshTokenPresenter)
        {
            _jwtOptions = jwtOptions.Value;
            _loginUseCase = loginUseCase;
            _loginPresenter = loginPresenter;
            _exchangeRefreshTokenUseCase = exchangeRefreshTokenUseCase;
            _exchangeRefreshTokenPresenter = exchangeRefreshTokenPresenter;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody]LoginModel model)
        {
            var request = new LoginRequest(model.UserName!, model.Password!, Request.HttpContext.Connection.RemoteIpAddress?.ToString());
            await _loginUseCase.Handle(request, _loginPresenter);

            return _loginPresenter.ContentResult;
        }

        [HttpPost("refreshtoken")]
        public async Task<ActionResult> RefreshToken([FromBody]ExchangeRefreshTokenModel model)
        {
            var request = new ExchangeRefreshTokenRequest(model.AccessToken!, model.RefreshToken!, _jwtOptions.SigningKey!);
            await _exchangeRefreshTokenUseCase.Handle(request, _exchangeRefreshTokenPresenter);
            return _exchangeRefreshTokenPresenter.ContentResult;
        }
    }
}