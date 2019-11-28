using ACS.API;
using ACS.API.Api.Authentication.Models;
using ACS.Core.Errors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ACS.IntegrationTests.Authentication
{
    public class AuthenticationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public AuthenticationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_GivenValidCredentials_Succeeds()
        {
            LoginModel model = new LoginModel { UserName = "acstestuser", Password = "Pa$$W0rd1" };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/authentication/login", content);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            dynamic result = JObject.Parse(stringResponse);
            Assert.NotNull(result.token);
            Assert.True((int)result.expiresIn > 0);
            Assert.NotNull(result.refreshToken);
        }

        [Fact]
        public async Task Login_GivenInvalidCredentials_Fails()
        {
            LoginModel model = new LoginModel { UserName = "unknown", Password = "fakepassword" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/authentication/login", content);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Contains(ACSErrors.Authentication.InvalidLogin, stringResponse);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ExchangeRefreshToken_GivenValidToken_Succeeds()
        {
            ExchangeRefreshTokenModel model = new ExchangeRefreshTokenModel { AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhY3N0ZXN0dXNlciIsImp0aSI6Ijg1ZjE5Yzc4LTVmNDgtNDJkOC1iNjdjLWI5ZGE1MjQ2NTc5MyIsImlhdCI6IjE1NjYyNDM3OTYiLCJyb2wiOiJ1c2VyIiwiaWQiOiIxIiwibmJmIjoxNTY2MjQzNzk1LCJleHAiOjE1NjYyNDczOTUsImlzcyI6IkFsaW1Db29wIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIn0.JL4v6wa4O9CyUWxSLuRzI0S9-IopobRM6zdDdfRTbeM", RefreshToken = "sf+BoDDXriyqkpwB4Wl3I59AYecTs9j37a981ijAGXs=" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/authentication/refreshtoken", content);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            dynamic result = JObject.Parse(stringResponse);
            Assert.NotNull(result.token);
            Assert.True((int)result.expiresIn > 0);
            Assert.NotNull(result.refreshToken);
        }

        [Fact]
        public async Task ExchangeRefreshToken_GivenInvalidToken_Fails()
        {
            ExchangeRefreshTokenModel model = new ExchangeRefreshTokenModel { AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhY3N0ZXN0dXNlciIsImp0aSI6Ijg1ZjE5Yzc4LTVmNDgtNDJkOC1iNjdjLWI5ZGE1MjQ2NTc5MyIsImlhdCI6IjE1NjYyNDM3OTYiLCJyb2wiOiJ1c2VyIiwiaWQiOiIxIiwibmJmIjoxNTY2MjQzNzk1LCJleHAiOjE1NjYyNDczOTUsImlzcyI6IkFsaW1Db29wIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIn0.JL4v6wa4O9CyUWxSLuRzI0S9-IopobRM6zdDdfRTbeM", RefreshToken = "unknown" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/authentication/refreshtoken", content);
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid token.", stringResponse);
        }
    }
}
