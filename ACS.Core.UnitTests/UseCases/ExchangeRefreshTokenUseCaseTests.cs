using ACS.Core.Entities;
using ACS.Core.Interfaces;
using ACS.Core.Interfaces.Filters;
using ACS.Core.Interfaces.Repositories;
using ACS.Core.Interfaces.UseCases;
using ACS.Core.QueryFilter;
using ACS.Core.UseCases.Authentication.Dto;
using ACS.Core.UseCases.Authentication.RefreshToken;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace ACS.Core.UnitTests.UseCases
{
    public class ExchangeRefreshTokenUseCaseTests
    {
        [Fact]
        public async void Handle_GivenInvalidToken_ShouldFail()
        {
            // arrange
            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(validator => validator.GetPrincipalFromToken(It.IsAny<string>(), It.IsAny<string>())).Returns((ClaimsPrincipal?)null);

            var mockOutputPort = new Mock<IOutputPort<ExchangeRefreshTokenResponse>>();
            mockOutputPort.Setup(outputPort => outputPort.Handle(It.IsAny<ExchangeRefreshTokenResponse>()));

            var useCase = new ExchangeRefreshTokenUseCase(tokenServiceMock.Object, null!);

            // act
            var response = await useCase.Handle(new ExchangeRefreshTokenRequest("", "", ""), mockOutputPort.Object);

            // assert
            Assert.False(response);
        }

        [Fact]
        public async void Handle_GivenValidToken_ShouldSucceed()
        {
            // arrange
            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>(), It.IsAny<string>())).Returns(new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(new []{ new Claim("id","1")})
            }));
            tokenServiceMock.Setup(x => x.GenerateEncodedToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new AccessToken("", 0, ""));
            tokenServiceMock.Setup(x => x.GenerateToken(32)).Returns("");

            const string refreshToken = "1234";
            var user = new ACSUser("", "", "", "");
            user.AddRefreshToken(refreshToken, 0, "");

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FirstOrDefaultByQueryAsync(It.IsAny<UserQueryFilter>())).ReturnsAsync(user);

            var mockOutputPort = new Mock<IOutputPort<ExchangeRefreshTokenResponse>>();
            mockOutputPort.Setup(outputPort => outputPort.Handle(It.IsAny<ExchangeRefreshTokenResponse>()));

            var useCase = new ExchangeRefreshTokenUseCase(tokenServiceMock.Object, mockUserRepository.Object);

            // act
            var response = await useCase.Handle(new ExchangeRefreshTokenRequest("", refreshToken, ""), mockOutputPort.Object);

            // assert
            Assert.True(response);
        }
    }
}
