using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RoboticVacuumCleaner.Web.Services;
using System.Text.Json;
using FluentAssertions;

namespace RoboticVacuumCleaner.Tests.Services.Client
{
    public class AuthServiceClientTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpHandler;
        private readonly Mock<ILocalStorageService> _mockLocalStorage;
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public AuthServiceClientTests()
        {
            _mockHttpHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            _mockLocalStorage = new Mock<ILocalStorageService>();
            _authService = new AuthService(_httpClient, _mockLocalStorage.Object);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnSuccess()
        {
            // Arrange
            var loginModel = new LoginModel
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var expectedResponse = new AuthResult
            {
                Success = true,
                Token = "test-token",
                RefreshToken = "refresh-token",
                User = new UserDto { Email = "test@example.com" }
            };

            var responseJson = JsonSerializer.Serialize(expectedResponse);

            _mockHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _authService.LoginAsync(loginModel);

            // Assert
            result.Success.Should().BeTrue();
            result.Token.Should().Be("test-token");
            _mockLocalStorage.Verify(x => x.SetItemAsync("authToken", "test-token"), Times.Once);
            _mockLocalStorage.Verify(x => x.SetItemAsync("refreshToken", "refresh-token"), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidCredentials_ShouldReturnFailure()
        {
            // Arrange
            var loginModel = new LoginModel
            {
                Email = "wrong@example.com",
                Password = "wrongpass"
            };

            var expectedResponse = new AuthResult
            {
                Success = false,
                Message = "Неверный email или пароль"
            };

            var responseJson = JsonSerializer.Serialize(expectedResponse);

            _mockHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _authService.LoginAsync(loginModel);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Неверный");
        }

        [Fact]
        public async Task LogoutAsync_ShouldClearTokens()
        {
            // Act
            await _authService.LogoutAsync();

            // Assert
            _mockLocalStorage.Verify(x => x.RemoveItemAsync("authToken"), Times.Once);
            _mockLocalStorage.Verify(x => x.RemoveItemAsync("refreshToken"), Times.Once);
        }

        [Fact]
        public async Task IsAuthenticatedAsync_WithToken_ShouldReturnTrue()
        {
            // Arrange
            _mockLocalStorage.Setup(x => x.GetItemAsync<string>("authToken"))
                .ReturnsAsync("valid-token");

            // Act
            var result = await _authService.IsAuthenticatedAsync();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsAuthenticatedAsync_WithoutToken_ShouldReturnFalse()
        {
            // Arrange
            _mockLocalStorage.Setup(x => x.GetItemAsync<string>("authToken"))
                .ReturnsAsync((string)null!);

            // Act
            var result = await _authService.IsAuthenticatedAsync();

            // Assert
            result.Should().BeFalse();
        }
    }
}