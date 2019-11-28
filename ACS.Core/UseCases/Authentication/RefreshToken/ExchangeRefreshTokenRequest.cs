using ACS.Core.Interfaces.UseCases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.UseCases.Authentication.RefreshToken
{
    public class ExchangeRefreshTokenRequest : IUseCaseRequest<ExchangeRefreshTokenResponse>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string SigningKey { get; set; }

        public ExchangeRefreshTokenRequest(string accessToken, string refreshToken, string signingKey)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            SigningKey = signingKey;
        }
    }
}