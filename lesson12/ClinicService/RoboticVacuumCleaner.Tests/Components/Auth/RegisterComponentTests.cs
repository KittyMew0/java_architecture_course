using Bunit;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.Win32;
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
    public class RegisterComponentTests : TestContext
    {
        private readonly Mock<IAuthService> _mockAuthService;

        public RegisterComponentTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            Services.AddScoped(_ => _mockAuthService.Object);
            Services.AddSingleton<NavigationManager>(Mock.Of<NavigationManager>());
        }

        [Fact]
        public void Register_ShouldRenderCorrectly()
        {
            // Act
            var cut = RenderComponent<Register>();

            // Assert
            cut.Find("h1").TextContent.Should().Contain("Регистрация");
            cut.Find("#firstName").Should().NotBeNull();
            cut.Find("#lastName").Should().NotBeNull();
            cut.Find("#email").Should().NotBeNull();
            cut.Find("#password").Should().NotBeNull();
        }

        [Fact]
        public async Task Register_WithValidData_ShouldCallAuthService()
        {
            // Arrange
            var expectedResult = new AuthResult
            {
                Success = true,
                Message = "Регистрация успешна"
            };

            _mockAuthService.Setup(x => x.RegisterAsync(It.IsAny<RegisterModel>()))
                .ReturnsAsync(expectedResult);

            var cut = RenderComponent<Register>();

            // Act
            cut.Find("#firstName").Change("Иван");
            cut.Find("#lastName").Change("Тестов");
            cut.Find("#email").Change("test@example.com");
            cut.Find("#password").Change("password123");
            cut.Find("form").Submit();

            await Task.Delay(100);

            // Assert
            _mockAuthService.Verify(x => x.RegisterAsync(It.Is<RegisterModel>(r =>
                r.FirstName == "Иван" &&
                r.LastName == "Тестов" &&
                r.Email == "test@example.com")),
                Times.Once);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ShouldShowErrorMessage()
        {
            // Arrange
            var expectedResult = new AuthResult
            {
                Success = false,
                Message = "Пользователь с таким email уже существует"
            };

            _mockAuthService.Setup(x => x.RegisterAsync(It.IsAny<RegisterModel>()))
                .ReturnsAsync(expectedResult);

            var cut = RenderComponent<Register>();

            // Act
            cut.Find("#firstName").Change("Иван");
            cut.Find("#lastName").Change("Тестов");
            cut.Find("#email").Change("existing@example.com");
            cut.Find("#password").Change("password123");
            cut.Find("form").Submit();

            await Task.Delay(100);

            // Assert
            cut.Find(".alert-danger").Should().NotBeNull();
            cut.Find(".alert-danger").TextContent.Should().Contain("уже существует");
        }
    }
}