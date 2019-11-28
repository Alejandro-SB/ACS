using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.UseCases.Authentication.Dto
{
    public class AccessToken
    {
        public string Token { get; }
        public int ExpiresIn { get; }
        public string RefreshToken { get; set; }

        public AccessToken(string token, int expiresIn, string refreshToken)
        {
            Token = token;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
        }
    }
}