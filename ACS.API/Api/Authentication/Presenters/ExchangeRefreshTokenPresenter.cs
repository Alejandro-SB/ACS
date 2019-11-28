using ACS.API.Api.Base;
using ACS.Core.Interfaces.UseCases;
using ACS.Core.UseCases.Authentication.RefreshToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ACS.API.Api.Authentication.Presenters
{
    public class ExchangeRefreshTokenPresenter : IOutputPort<ExchangeRefreshTokenResponse>
    {
        public JsonContentResult ContentResult { get; set; }

        public ExchangeRefreshTokenPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(ExchangeRefreshTokenResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = response.Success ? JsonSerializer.SerializeObject(response.AccessToken) : JsonSerializer.SerializeObject(response.Message);
        }
    }
}
