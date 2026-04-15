using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RoboticVacuumCleaner.Server.Data;
using RoboticVacuumCleaner.Server.DTOs;
using RoboticVacuumCleaner.Server.Models;
using RoboticVacuumCleaner.Server.Services;
using FluentAssertions;
using Xunit;

namespace RoboticVacuumCleaner.Tests.Services
{
    public class AuthServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ILogger<AuthService>> _mockLogger;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["JWT:Secret"])
                .Returns("test-secret-key-with-minimum-32-characters!");

            _mockEmailService = new Mock<IEmailService>();
            _mockLogger = new Mock<ILogger<AuthService>>();

            _authService = new AuthService(
                _context,
                _mockConfiguration.Object,
                _mockEmailService.Object,
                _mockLogger.Object);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "Иван",
                LastName = "Тестов",
                PhoneNumber = "+79991234567"
            };

            // Act
            var result = await _authService.RegisterAsync(request);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Contain("Регистрация успешна");
            result.Token.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
            result.User.Should().NotBeNull();
            result.User!.Email.Should().Be("test@example.com");
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ShouldReturnFailure()
        {
            // Arrange
            var existingUser = new User
            {
                Email = "existing@example.com",
                FirstName = "Существующий",
                LastName = "Пользователь",
                PasswordHash = "hash"
            };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var request = new RegisterRequest
            {
                Email = "existing@example.com",
                Password = "password123",
                FirstName = "Новый",
                LastName = "Пользователь"
            };

            // Act
            var result = await _authService.RegisterAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("уже существует");
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnSuccess()
        {
            // Arrange
            var user = new User
            {
                Email = "test@example.com",
                FirstName = "Иван",
                LastName = "Тестов",
                PasswordHash = HashPassword("password123"),
                IsEmailConfirmed = true
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Success.Should().BeTrue();
            result.Token.Should().NotBeNullOrEmpty();
            result.User.Should().NotBeNull();
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ShouldReturnFailure()
        {
            // Arrange
            var user = new User
            {
                Email = "test@example.com",
                FirstName = "Иван",
                LastName = "Тестов",
                PasswordHash = HashPassword("correctpassword"),
                IsEmailConfirmed = true
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Неверный");
        }

        [Fact]
        public async Task LoginAsync_WithUnconfirmedEmail_ShouldReturnFailure()
        {
            // Arrange
            var user = new User
            {
                Email = "test@example.com",
                FirstName = "Иван",
                LastName = "Тестов",
                PasswordHash = HashPassword("password123"),
                IsEmailConfirmed = false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("подтвердите");
        }

        [Fact]
        public async Task ChangePasswordAsync_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Email = "test@example.com",
                FirstName = "Иван",
                LastName = "Тестов",
                PasswordHash = HashPassword("oldpassword")
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var request = new ChangePasswordRequest
            {
                CurrentPassword = "oldpassword",
                NewPassword = "newpassword123",
                ConfirmPassword = "newpassword123"
            };

            // Act
            var result = await _authService.ChangePasswordAsync(1, request);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Contain("успешно");
        }

        [Fact]
        public async Task ChangePasswordAsync_WithWrongCurrentPassword_ShouldReturnFailure()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Email = "test@example.com",
                FirstName = "Иван",
                LastName = "Тестов",
                PasswordHash = HashPassword("correctpassword")
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var request = new ChangePasswordRequest
            {
                CurrentPassword = "wrongpassword",
                NewPassword = "newpassword123",
                ConfirmPassword = "newpassword123"
            };

            // Act
            var result = await _authService.ChangePasswordAsync(1, request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Неверный");
        }

        [Fact]
        public async Task ForgotPasswordAsync_WithExistingEmail_ShouldSendEmail()
        {
            // Arrange
            var user = new User
            {
                Email = "test@example.com",
                FirstName = "Иван",
                LastName = "Тестов",
                PasswordHash = HashPassword("password")
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var request = new ForgotPasswordRequest
            {
                Email = "test@example.com"
            };

            // Act
            var result = await _authService.ForgotPasswordAsync(request);

            // Assert
            result.Success.Should().BeTrue();
            _mockEmailService.Verify(
                x => x.SendPasswordResetEmailAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}