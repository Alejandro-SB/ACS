using ACS.Core.Entities;
using ACS.Core.Interfaces;
using ACS.Core.Interfaces.Repositories;
using ACS.Core.Interfaces.UseCases;
using ACS.Core.UseCases.Authentication.Dto;
using ACS.Core.UseCases.Authentication.Login;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ACS.Core.UnitTests.UseCases
{
    public class LoginUseCaseTests
    {
        [Fact]
        public async void Handle_GivenValidCredentials_Succeeds()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ACSUser("", "", "", ""));

            mockUserRepository.Setup(repo => repo.CheckPasswordAsync(It.IsAny<ACSUser>(), It.IsAny<string>())).ReturnsAsync(true);

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(factory => factory.GenerateEncodedToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new AccessToken("", 0, ""));

            var useCase = new LoginUseCase(mockUserRepository.Object, tokenServiceMock.Object);

            var mockOutputPort = new Mock<IOutputPort<LoginResponse>>();
            mockOutputPort.Setup(outputPort => outputPort.Handle(It.IsAny<LoginResponse>()));

            // act
            var response = await useCase.Handle(new LoginRequest("userName", "password", "127.0.0.1"), mockOutputPort.Object);

            // assert
            Assert.True(response);
        }

        [Fact]
        public async void Handle_GivenIncompleteCredentials_Fails()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ACSUser("", "", "", ""));

            mockUserRepository.Setup(repo => repo.CheckPasswordAsync(It.IsAny<ACSUser>(), It.IsAny<string>())).ReturnsAsync(false);

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(factory => factory.GenerateEncodedToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new AccessToken("", 0, ""));

            var useCase = new LoginUseCase(mockUserRepository.Object, tokenServiceMock.Object);

            var mockOutputPort = new Mock<IOutputPort<LoginResponse>>();
            mockOutputPort.Setup(outputPort => outputPort.Handle(It.IsAny<LoginResponse>()));

            // act
            var response = await useCase.Handle(new LoginRequest("", "password", "127.0.0.1"), mockOutputPort.Object);

            // assert
            Assert.False(response);
            tokenServiceMock.Verify(factory => factory.GenerateToken(32), Times.Never);
        }


        [Fact]
        public async void Handle_GivenUnknownCredentials_Fails()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ACSUser?)null);

            mockUserRepository.Setup(repo => repo.CheckPasswordAsync(It.IsAny<ACSUser>(), It.IsAny<string>())).ReturnsAsync(true);

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(factory => factory.GenerateEncodedToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new AccessToken("", 0, ""));

            var useCase = new LoginUseCase(mockUserRepository.Object, tokenServiceMock.Object);

            var mockOutputPort = new Mock<IOutputPort<LoginResponse>>();
            mockOutputPort.Setup(outputPort => outputPort.Handle(It.IsAny<LoginResponse>()));

            // act
            var response = await useCase.Handle(new LoginRequest("", "password", "127.0.0.1"), mockOutputPort.Object);

            // assert
            Assert.False(response);
            tokenServiceMock.Verify(factory => factory.GenerateToken(32), Times.Never);
        }

        [Fact]
        public async void Handle_GivenInvalidPassword_Fails()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ACSUser?)null);

            mockUserRepository.Setup(repo => repo.CheckPasswordAsync(It.IsAny<ACSUser>(), It.IsAny<string>())).ReturnsAsync(false);

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(factory => factory.GenerateEncodedToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new AccessToken("", 0, ""));

            var useCase = new LoginUseCase(mockUserRepository.Object, tokenServiceMock.Object);

            var mockOutputPort = new Mock<IOutputPort<LoginResponse>>();
            mockOutputPort.Setup(outputPort => outputPort.Handle(It.IsAny<LoginResponse>()));

            // act
            var response = await useCase.Handle(new LoginRequest("", "password", "127.0.0.1"), mockOutputPort.Object);

            // assert
            Assert.False(response);
            tokenServiceMock.Verify(factory => factory.GenerateToken(32), Times.Never);
        }
    }
}