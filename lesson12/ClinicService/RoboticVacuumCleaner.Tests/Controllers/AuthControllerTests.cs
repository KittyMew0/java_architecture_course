using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboticVacuumCleaner.Server.Controllers;
using RoboticVacuumCleaner.Server.DTOs;
using RoboticVacuumCleaner.Server.Services;
using FluentAssertions;
using Xunit;

namespace RoboticVacuumCleaner.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object,
                Mock.Of<ILogger<AuthController>>());
        }

        [Fact]
        public async Task Register_WithValidRequest_ShouldReturnOk()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "Иван",
                LastName = "Тестов"
            };

            var expectedResult = new AuthResponse
            {
                Success = true,
                Message = "Регистрация успешна",
                Token = "test-token",
                User = new UserDto { Email = "test@example.com" }
            };

            _mockAuthService.Setup(x => x.RegisterAsync(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<AuthResponse>().Subject;
            response.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Register_WithInvalidModel_ShouldReturnBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Email is required");
            var request = new RegisterRequest();

            // Act
            var result = await _controller.Register(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnOk()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var expectedResult = new AuthResponse
            {
                Success = true,
                Token = "test-token",
                User = new UserDto { Email = "test@example.com" }
            };

            _mockAuthService.Setup(x => x.LoginAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<AuthResponse>().Subject;
            response.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var expectedResult = new AuthResponse
            {
                Success = false,
                Message = "Неверный email или пароль"
            };

            _mockAuthService.Setup(x => x.LoginAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Login(request);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task RefreshToken_WithValidToken_ShouldReturnOk()
        {
            // Arrange
            var request = new RefreshTokenRequest { RefreshToken = "valid-refresh-token" };

            var expectedResult = new AuthResponse
            {
                Success = true,
                Token = "new-token",
                RefreshToken = "new-refresh-token"
            };

            _mockAuthService.Setup(x => x.RefreshTokenAsync(It.IsAny<RefreshTokenRequest>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.RefreshToken(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ChangePassword_WithValidRequest_ShouldReturnOk()
        {
            // Arrange
            var request = new ChangePasswordRequest
            {
                CurrentPassword = "oldpass",
                NewPassword = "newpass123",
                ConfirmPassword = "newpass123"
            };

            var expectedResult = new AuthResponse
            {
                Success = true,
                Message = "Пароль успешно изменен"
            };

            _mockAuthService.Setup(x => x.ChangePasswordAsync(It.IsAny<int>(), It.IsAny<ChangePasswordRequest>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ChangePassword(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<AuthResponse>().Subject;
            response.Success.Should().BeTrue();
        }

        [Fact]
        public async Task ForgotPassword_WithValidEmail_ShouldReturnOk()
        {
            // Arrange
            var request = new ForgotPasswordRequest { Email = "test@example.com" };

            var expectedResult = new AuthResponse
            {
                Success = true,
                Message = "Инструкции отправлены"
            };

            _mockAuthService.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordRequest>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ForgotPassword(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<AuthResponse>().Subject;
            response.Success.Should().BeTrue();
        }
    }
}