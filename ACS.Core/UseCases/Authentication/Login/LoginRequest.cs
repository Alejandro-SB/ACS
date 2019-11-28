using ACS.Core.Interfaces.UseCases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.UseCases.Authentication.Login
{
    public class LoginRequest : IUseCaseRequest<LoginResponse>
    {
        public string UserName { get; }
        public string Password { get; }
        public string? Ip { get; }

        public LoginRequest(string userName, string password, string? ip)
        {
            UserName = userName;
            Password = password;
            Ip = ip;
        }
    }
}