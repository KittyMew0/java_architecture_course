using FluentAssertions;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using RoboticVacuumCleaner.Server.DTOs;
using FluentAssertions;
using Xunit;

namespace RoboticVacuumCleaner.Tests.Models
{
    public class ValidationTests
    {
        [Fact]
        public void RegisterRequest_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "Иван",
                LastName = "Тестов"
            };

            // Act
            var validationResults = ValidateModel(request);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void RegisterRequest_WithoutEmail_ShouldFailValidation()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Password = "password123",
                FirstName = "Иван",
                LastName = "Тестов"
            };

            // Act
            var validationResults = ValidateModel(request);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(v => v.MemberNames.Contains("Email"));
        }

        [Fact]
        public void RegisterRequest_WithInvalidEmail_ShouldFailValidation()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "invalid-email",
                Password = "password123",
                FirstName = "Иван",
                LastName = "Тестов"
            };

            // Act
            var validationResults = ValidateModel(request);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(v => v.MemberNames.Contains("Email"));
        }

        [Fact]
        public void RegisterRequest_WithShortPassword_ShouldFailValidation()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "123",
                FirstName = "Иван",
                LastName = "Тестов"
            };

            // Act
            var validationResults = ValidateModel(request);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(v => v.MemberNames.Contains("Password"));
        }

        [Fact]
        public void ChangePasswordRequest_WithMismatchedPasswords_ShouldFailValidation()
        {
            // Arrange
            var request = new ChangePasswordRequest
            {
                CurrentPassword = "oldpass",
                NewPassword = "newpass123",
                ConfirmPassword = "differentpass"
            };

            // Act
            var validationResults = ValidateModel(request);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(v => v.MemberNames.Contains("ConfirmPassword"));
        }

        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, validationResults, true);
            return validationResults;
        }
    }
}