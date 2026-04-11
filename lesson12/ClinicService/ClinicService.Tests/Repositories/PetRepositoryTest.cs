using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClinicService.Models;
using FluentAssertions;
using Xunit;

namespace ClinicService.Tests.Repositories
{
    public class PetRepositoryTests : BaseRepositoryTests
    {
        private Client CreateTestClient()
        {
            var client = new Client
            {
                Document = "1234-567890",
                SurName = "Иванов",
                FirstName = "Иван",
                Birthday = new DateTime(1990, 1, 1)
            };
            _clientRepository.Create(client);
            return client;
        }

        [Fact]
        public void Create_ShouldAddNewPet()
        {
            // Arrange
            var client = CreateTestClient();
            var pet = new Pet
            {
                ClientId = client.ClientId,
                Name = "Бобик",
                Birthday = new DateTime(2020, 1, 1)
            };

            // Act
            int result = _petRepository.Create(pet);

            // Assert
            result.Should().Be(1);

            var createdPet = _petRepository.GetById(pet.PetId);
            createdPet.Should().NotBeNull();
            createdPet.Name.Should().Be("Бобик");
            createdPet.ClientId.Should().Be(client.ClientId);
        }

        [Fact]
        public void Update_ShouldModifyExistingPet()
        {
            // Arrange
            var client = CreateTestClient();
            var pet = new Pet
            {
                ClientId = client.ClientId,
                Name = "Бобик",
                Birthday = new DateTime(2020, 1, 1)
            };
            _petRepository.Create(pet);

            // Act
            pet.Name = "Шарик";
            int result = _petRepository.Update(pet);

            // Assert
            result.Should().Be(1);

            var updatedPet = _petRepository.GetById(pet.PetId);
            updatedPet.Name.Should().Be("Шарик");
        }

        [Fact]
        public void Delete_ShouldRemovePet()
        {
            // Arrange
            var client = CreateTestClient();
            var pet = new Pet
            {
                ClientId = client.ClientId,
                Name = "Бобик",
                Birthday = new DateTime(2020, 1, 1)
            };
            _petRepository.Create(pet);

            // Act
            int result = _petRepository.Delete(pet.PetId);

            // Assert
            result.Should().Be(1);

            var deletedPet = _petRepository.GetById(pet.PetId);
            deletedPet.Should().BeNull();
        }

        [Fact]
        public void GetByClientId_ShouldReturnClientPets()
        {
            // Arrange
            var client1 = CreateTestClient();
            var client2 = CreateTestClient();

            var pet1 = new Pet { ClientId = client1.ClientId, Name = "Бобик", Birthday = DateTime.Now };
            var pet2 = new Pet { ClientId = client1.ClientId, Name = "Мурка", Birthday = DateTime.Now };
            var pet3 = new Pet { ClientId = client2.ClientId, Name = "Шарик", Birthday = DateTime.Now };

            _petRepository.Create(pet1);
            _petRepository.Create(pet2);
            _petRepository.Create(pet3);

            // Act
            var allPets = _petRepository.GetAll();
            var client1Pets = allPets.Where(p => p.ClientId == client1.ClientId).ToList();

            // Assert
            client1Pets.Should().HaveCount(2);
            client1Pets.Should().Contain(p => p.Name == "Бобик");
            client1Pets.Should().Contain(p => p.Name == "Мурка");
        }
    }
}