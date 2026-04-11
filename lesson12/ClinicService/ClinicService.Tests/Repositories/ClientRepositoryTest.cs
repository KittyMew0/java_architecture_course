using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClinicService.Models;
using ClinicService.Services.Repositories;
using FluentAssertions;
using Xunit;

namespace ClinicService.Tests.Repositories
{
    public class ClientRepositoryTests : BaseRepositoryTests
    {
        [Fact]
        public void Create_ShouldAddNewClient()
        {
            // Arrange
            var client = new Client
            {
                Document = "1234-567890",
                SurName = "Иванов",
                FirstName = "Иван",
                Patronymic = "Иванович",
                Birthday = new DateTime(1990, 1, 1)
            };

            // Act
            int result = _clientRepository.Create(client);

            // Assert
            result.Should().Be(1);

            var createdClient = _clientRepository.GetById(client.ClientId);
            createdClient.Should().NotBeNull();
            createdClient.Document.Should().Be("1234-567890");
            createdClient.SurName.Should().Be("Иванов");
            createdClient.FirstName.Should().Be("Иван");
        }

        [Fact]
        public void Update_ShouldModifyExistingClient()
        {
            // Arrange
            var client = new Client
            {
                Document = "1234-567890",
                SurName = "Иванов",
                FirstName = "Иван",
                Patronymic = "Иванович",
                Birthday = new DateTime(1990, 1, 1)
            };
            _clientRepository.Create(client);

            // Act
            client.SurName = "Петров";
            client.FirstName = "Петр";
            int result = _clientRepository.Update(client);

            // Assert
            result.Should().Be(1);

            var updatedClient = _clientRepository.GetById(client.ClientId);
            updatedClient.SurName.Should().Be("Петров");
            updatedClient.FirstName.Should().Be("Петр");
        }

        [Fact]
        public void Delete_ShouldRemoveClient()
        {
            // Arrange
            var client = new Client
            {
                Document = "1234-567890",
                SurName = "Иванов",
                FirstName = "Иван",
                Patronymic = "Иванович",
                Birthday = new DateTime(1990, 1, 1)
            };
            _clientRepository.Create(client);

            // Act
            int result = _clientRepository.Delete(client.ClientId);

            // Assert
            result.Should().Be(1);

            var deletedClient = _clientRepository.GetById(client.ClientId);
            deletedClient.Should().BeNull();
        }

        [Fact]
        public void GetById_ShouldReturnCorrectClient()
        {
            // Arrange
            var client1 = new Client
            {
                Document = "1111-111111",
                SurName = "Иванов",
                FirstName = "Иван",
                Birthday = new DateTime(1990, 1, 1)
            };
            var client2 = new Client
            {
                Document = "2222-222222",
                SurName = "Петров",
                FirstName = "Петр",
                Birthday = new DateTime(1995, 5, 5)
            };
            _clientRepository.Create(client1);
            _clientRepository.Create(client2);

            // Act
            var result = _clientRepository.GetById(client2.ClientId);

            // Assert
            result.Should().NotBeNull();
            result.Document.Should().Be("2222-222222");
            result.SurName.Should().Be("Петров");
        }

        [Fact]
        public void GetAll_ShouldReturnAllClients()
        {
            // Arrange
            var client1 = new Client
            {
                Document = "1111-111111",
                SurName = "Иванов",
                FirstName = "Иван",
                Birthday = new DateTime(1990, 1, 1)
            };
            var client2 = new Client
            {
                Document = "2222-222222",
                SurName = "Петров",
                FirstName = "Петр",
                Birthday = new DateTime(1995, 5, 5)
            };
            _clientRepository.Create(client1);
            _clientRepository.Create(client2);

            // Act
            var clients = _clientRepository.GetAll();

            // Assert
            clients.Should().HaveCount(2);
            clients.Should().Contain(c => c.SurName == "Иванов");
            clients.Should().Contain(c => c.SurName == "Петров");
        }
    }
}