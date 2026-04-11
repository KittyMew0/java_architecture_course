using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using ClinicService.Models.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ClinicService.Tests.Integration
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task ClientController_CreateAndGet_ShouldWork()
        {
            // Arrange
            var createRequest = new CreateClientRequest
            {
                Document = "1234-567890",
                SurName = "Иванов",
                FirstName = "Иван",
                Birthday = new DateTime(1990, 1, 1)
            };

            var json = JsonSerializer.Serialize(createRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act - Create
            var createResponse = await _client.PostAsync("/api/Client/create", content);
            createResponse.EnsureSuccessStatusCode();

            // Act - Get All
            var getAllResponse = await _client.GetAsync("/api/Client/get-all");
            getAllResponse.EnsureSuccessStatusCode();

            var responseString = await getAllResponse.Content.ReadAsStringAsync();

            // Assert
            createResponse.IsSuccessStatusCode.Should().BeTrue();
            getAllResponse.IsSuccessStatusCode.Should().BeTrue();
            responseString.Should().NotBeEmpty();
        }
    }
}