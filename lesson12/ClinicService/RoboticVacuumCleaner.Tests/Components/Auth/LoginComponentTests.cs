using Bunit;
using Microsoft.AspNetCore.Components;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RoboticVacuumCleaner.Tests.Helpers;
using RoboticVacuumCleaner.Web.Pages;
using RoboticVacuumCleaner.Web.Services;

namespace RoboticVacuumCleaner.Tests.Components.Auth
{
    public class LoginComponentTests : TestContext
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<NavigationManager> _mockNavigationManager;

        public LoginComponentTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockNavigationManager = new Mock<NavigationManager>();

            Services.AddScoped(_ => _mockAuthService.Object);
            Services.AddSingleton(_mockNavigationManager.Object);
        }

        [Fact]
        public void Login_ShouldRenderCorrectly()
        {
            // Act
            var cut = RenderComponent<Login>();

            // Assert
            cut.Find("h1").TextContent.Should().Contain("Вход в систему");
            cut.Find("form").Should().NotBeNull();
            cut.Find("#email").Should().NotBeNull();
            cut.Find("#password").Should().NotBeNull();
            cut.Find("button[type=submit]").TextContent.Should().Contain("Войти");
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldCallAuthService()
        {
            // Arrange
            var expectedResult = new AuthResult
            {
                Success = true,
                Token = "test-token",
                RefreshToken = "test-refresh-token",
                User = new UserDto { Email = "test@example.com" }
            };

            _mockAuthService.Setup(x => x.LoginAsync(It.IsAny<LoginModel>()))
                .ReturnsAsync(expectedResult);

            var cut = RenderComponent<Login>();

            // Act
            cut.Find("#email").Change("test@example.com");
            cut.Find("#password").Change("password123");
            cut.Find("form").Submit();

            await Task.Delay(100);

            // Assert
            _mockAuthService.Verify(x => x.LoginAsync(It.Is<LoginModel>(l =>
                l.Email == "test@example.com" && l.Password == "password123")),
                Times.Once);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldShowErrorMessage()
        {
            // Arrange
            var expectedResult = new AuthResult
            {
                Success = false,
                Message = "Неверный email или пароль"
            };

            _mockAuthService.Setup(x => x.LoginAsync(It.IsAny<LoginModel>()))
                .ReturnsAsync(expectedResult);

            var cut = RenderComponent<Login>();

            // Act
            cut.Find("#email").Change("wrong@example.com");
            cut.Find("#password").Change("wrongpass");
            cut.Find("form").Submit();

            await Task.Delay(100);

            // Assert
            cut.Find(".alert-danger").Should().NotBeNull();
            cut.Find(".alert-danger").TextContent.Should().Contain("Неверный email или пароль");
        }

        [Fact]
        public void Login_WhenAlreadyAuthenticated_ShouldRedirectToDashboard()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(x => x.IsAuthenticatedAsync()).ReturnsAsync(true);

            Services.AddScoped(_ => mockAuthService.Object);

            // Act
            var cut = RenderComponent<Login>();

            // Assert
            _mockNavigationManager.Verify(x => x.NavigateTo("/dashboard", false, true), Times.AtLeastOnce);
        }
    }
}