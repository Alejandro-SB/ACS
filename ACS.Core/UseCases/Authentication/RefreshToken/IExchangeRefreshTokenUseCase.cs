using ACS.Core.Interfaces.UseCases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.UseCases.Authentication.RefreshToken
{
    public interface IExchangeRefreshTokenUseCase : IUseCaseRequestHandler<ExchangeRefreshTokenRequest, ExchangeRefreshTokenResponse>
    {
    }
}
