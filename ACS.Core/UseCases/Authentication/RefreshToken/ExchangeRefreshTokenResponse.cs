using ACS.Core.UseCases.Authentication.Dto;
using ACS.Core.UseCases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.UseCases.Authentication.RefreshToken
{
    public class ExchangeRefreshTokenResponse : BaseUseCaseResponse
    {
        public AccessToken? AccessToken { get; set; }

        public ExchangeRefreshTokenResponse(bool success = false, string? message = null) : base(success, message) { }

        public ExchangeRefreshTokenResponse(AccessToken token, bool success = false, string? message = null) : base(success, message)
        {
            AccessToken = token;
        }
    }
}