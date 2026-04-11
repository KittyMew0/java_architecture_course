using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClinicService.Controllers;
using ClinicService.Models;
using ClinicService.Models.Requests;
using ClinicService.Models.Responses;
using ClinicService.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicService.Tests.Controllers
{
    public class ClientControllerTests
    {
        private readonly Mock<IClientRepository> _mockRepository;
        private readonly ClientController _controller;

        public ClientControllerTests()
        {
            _mockRepository = new Mock<IClientRepository>();
            _controller = new ClientController(_mockRepository.Object);
        }

        [Fact]
        public void Create_WithValidRequest_ShouldReturnOk()
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

            _mockRepository.Setup(r => r.Create(It.IsAny<Client>()))
                .Returns(1)
                .Callback<Client>(c => c.ClientId = 1);

            // Act
            var result = _controller.Create(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            _mockRepository.Verify(r => r.Create(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public void Create_WithNullRequest_ShouldReturnBadRequest()
        {
            // Act
            var result = _controller.Create(null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Update_WithValidRequest_ShouldReturnOk()
        {
            // Arrange
            var request = new UpdateClientRequest
            {
                ClientId = 1,
                Document = "1234-567890",
                SurName = "Петров",
                FirstName = "Петр",
                Birthday = new DateTime(1990, 1, 1)
            };

            var existingClient = new Client { ClientId = 1, SurName = "Иванов" };
            _mockRepository.Setup(r => r.GetById(1)).Returns(existingClient);
            _mockRepository.Setup(r => r.Update(It.IsAny<Client>())).Returns(1);

            // Act
            var result = _controller.Update(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Update_WithNonExistentClient_ShouldReturnNotFound()
        {
            // Arrange
            var request = new UpdateClientRequest { ClientId = 999, SurName = "Петров" };
            _mockRepository.Setup(r => r.GetById(999)).Returns((Client)null);

            // Act
            var result = _controller.Update(request);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnOk()
        {
            // Arrange
            var client = new Client { ClientId = 1, SurName = "Иванов" };
            _mockRepository.Setup(r => r.GetById(1)).Returns(client);
            _mockRepository.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var result = _controller.Delete(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetAll_ShouldReturnAllClients()
        {
            // Arrange
            var clients = new List<Client>
            {
                new() { ClientId = 1, SurName = "Иванов", FirstName = "Иван" },
                new() { ClientId = 2, SurName = "Петров", FirstName = "Петр" }
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(clients);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedClients = okResult.Value.Should().BeAssignableTo<IEnumerable<ClientResponse>>().Subject;
            returnedClients.Should().HaveCount(2);
        }
    }
}