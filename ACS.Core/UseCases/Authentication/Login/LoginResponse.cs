using ACS.Core.UseCases.Authentication.Dto;
using ACS.Core.UseCases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.UseCases.Authentication.Login
{
    public class LoginResponse : BaseUseCaseResponse
    {
        public AccessToken? AccessToken { get; }
        public IEnumerable<string>? ErrorCodes { get; }

        public LoginResponse(IEnumerable<string> errors, bool success = false, string? message = null) : base(success, message)
        {
            ErrorCodes = errors;
        }

        public LoginResponse(AccessToken token, bool success = false, string? message = null) : base(success, message)
        {
            AccessToken = token;
        }
    }
}