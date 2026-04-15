using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using RoboticVacuumCleaner.Server;
using RoboticVacuumCleaner.Server.DTOs;
using FluentAssertions;
using Xunit;

namespace RoboticVacuumCleaner.Tests.Integration
{
    public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AuthIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task RegisterAndLogin_ShouldWorkCorrectly()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = $"test{Guid.NewGuid()}@example.com",
                Password = "TestPassword123!",
                FirstName = "Иван",
                LastName = "Тестов"
            };

            // Act - Register
            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

            // Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var registerResult = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
            registerResult.Should().NotBeNull();
            registerResult!.Success.Should().BeTrue();

            // Act - Login
            var loginRequest = new LoginRequest
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

            // Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
            loginResult.Should().NotBeNull();
            loginResult!.Success.Should().BeTrue();
            loginResult.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "nonexistent@example.com",
                Password = "WrongPassword123!"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}