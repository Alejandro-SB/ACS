using ACS.Core.Interfaces.UseCases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.UseCases.Authentication.Login
{
    public interface ILoginUseCase : IUseCaseRequestHandler<LoginRequest, LoginResponse>
    {
    }
}