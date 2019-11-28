using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACS.API.Api.Authentication.Models
{
    public class ExchangeRefreshTokenModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}