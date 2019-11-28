using ACS.API.Api.Base;
using ACS.Core.Interfaces.UseCases;
using ACS.Core.UseCases.Authentication.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ACS.API.Api.Authentication.Presenters
{
    public sealed class LoginPresenter : IOutputPort<LoginResponse>
    {
        public JsonContentResult ContentResult { get; }

        public LoginPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(LoginResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.Unauthorized);
            ContentResult.Content = response.Success ? JsonSerializer.SerializeObject(response.AccessToken) : JsonSerializer.SerializeObject(response.ErrorCodes);
        }
    }
}