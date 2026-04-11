using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClinicService.Models.Requests;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ClinicService.Tests.Models
{
    public class RequestValidationTests
    {
        [Fact]
        public void CreateClientRequest_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var request = new CreateClientRequest
            {
                Document = "1234-567890",
                SurName = "Иванов",
                FirstName = "Иван",
                Patronymic = "Иванович",
                Birthday = new DateTime(1990, 1, 1)
            };

            // Act
            var validationResults = ValidateModel(request);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void CreateClientRequest_WithoutRequiredFields_ShouldFailValidation()
        {
            // Arrange
            var request = new CreateClientRequest
            {
                Document = "1234-567890"
                // Missing SurName, FirstName, Birthday
            };

            // Act
            var validationResults = ValidateModel(request);

            // Assert
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(v => v.MemberNames.Contains("SurName"));
            validationResults.Should().Contain(v => v.MemberNames.Contains("FirstName"));
        }

        [Fact]
        public void CreatePetRequest_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var request = new CreatePetRequest
            {
                ClientId = 1,
                Name = "Бобик",
                Birthday = new DateTime(2020, 1, 1)
            };

            // Act
            var validationResults = ValidateModel(request);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void CreateConsultationRequest_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var request = new CreateConsultationRequest
            {
                ClientId = 1,
                PetId = 1,
                ConsultationDate = DateTime.Now,
                Description = "Плановый осмотр"
            };

            // Act
            var validationResults = ValidateModel(request);

            // Assert
            validationResults.Should().BeEmpty();
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