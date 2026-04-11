using ClinicService.Models;
using ClinicService.Models.Requests;
using ClinicService.Models.Responses;
using ClinicService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly IPetRepository _petRepository;
        private readonly IClientRepository _clientRepository;

        public PetController(IPetRepository petRepository, IClientRepository clientRepository)
        {
            _petRepository = petRepository;
            _clientRepository = clientRepository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreatePetRequest createRequest)
        {
            if (createRequest == null)
                return BadRequest("Запрос не может быть пустым");

            var client = _clientRepository.GetById(createRequest.ClientId);
            if (client == null)
                return BadRequest($"Клиент с {createRequest.ClientId} идентификатором не найден");

            var pet = new Pet
            {
                ClientId = createRequest.ClientId,
                Name = createRequest.Name,
                Birthday = createRequest.Birthday,
            };

            int res = _petRepository.Create(pet);

            if (res > 0)
                return Ok(new { Message = "Питомец добавлен успешно", Id = pet.PetId });

            return StatusCode(500, "Ошибка при добавлении питомца");
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdatePetRequest updateRequest)
        {
            if (updateRequest == null)
                return BadRequest("Запрос не может быть пустым");

            var existingPet = _petRepository.GetById(updateRequest.PetId);
            if (existingPet == null)
                return NotFound($"Питомец с {updateRequest.PetId} идентификатором не найден");

            var client = _clientRepository.GetById(updateRequest.ClientId);
            if (client == null)
                return BadRequest($"Клиент с {updateRequest.ClientId} идентификатором не найден");

            var pet = new Pet
            {
                PetId = updateRequest.PetId,
                ClientId = updateRequest.ClientId,
                Name = updateRequest.Name,
                Birthday = updateRequest.Birthday,
            };

            int res = _petRepository.Update(pet);

            if (res > 0)
                return Ok(new { Message = "Данные питомца обновлены успешно" });

            return StatusCode(500, "Ошибка при обновлении данных питомца");
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int petId)
        {
            var existingPet = _petRepository.GetById(petId);
            if (existingPet == null)
                return NotFound($"Питомец с {petId} идентификатором не найден");

            int res = _petRepository.Delete(petId);

            if (res > 0)
                return Ok(new { Message = "Данные о питомце удалены успешно" });

            return StatusCode(500, "Ошибка при удалении данных питомца");
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var pets = _petRepository.GetAll();

            var response = pets.Select(p => new PetResponse
            {
                PetId = p.PetId,
                ClientId = p.ClientId,
                Name = p.Name,
                Birthday = p.Birthday
            }).ToList();

            return Ok(response);
        }

        [HttpGet("get/{petId}")]
        public IActionResult GetById([FromRoute] int petId)
        {
            var pet = _petRepository.GetById(petId);

            if (pet == null)
                return NotFound($"Питомец с {petId} идентификатором");

            var response = new PetResponse
            {
                PetId = pet.PetId,
                ClientId = pet.ClientId,
                Name = pet.Name,
                Birthday = pet.Birthday
            };

            return Ok(response);
        }

        [HttpGet("get-by-client/{clientId}")]
        public IActionResult GetByClientId([FromRoute] int clientId)
        {
            var client = _clientRepository.GetById(clientId);
            if (client == null)
                return NotFound($"Клиент с {clientId} идентификатором не найден");

            var pets = _petRepository.GetAll()
                .Where(p => p.ClientId == clientId)
                .ToList();

            var response = pets.Select(p => new PetResponse
            {
                PetId = p.PetId,
                ClientId = p.ClientId,
                Name = p.Name,
                Birthday = p.Birthday
            }).ToList();

            return Ok(response);
        }

        [HttpGet("get-by-client/{clientId}")]
        public IActionResult GetByClientId([FromRoute] int clientId)
        {
            var client = _clientRepository.GetById(clientId);
            if (client == null)
                return NotFound($"Клиент с {clientId} идентификатором не найден");

            var pets = _petRepository.GetByClientId(clientId);
            return Ok(pets);
        }

        [HttpGet("search-by-name")]
        public IActionResult SearchByName([FromQuery] string name)
        {
            var pets = _petRepository.GetByNameContains(name);
            return Ok(pets);
        }

        [HttpGet("youngest/{count}")]
        public IActionResult GetYoungestPets(int count)
        {
            var pets = _petRepository.GetYoungestPets(count);
            return Ok(pets);
        }

        [HttpGet("stats/by-client/{clientId}")]
        public IActionResult GetPetCountByClient(int clientId)
        {
            var count = _petRepository.CountByClientId(clientId);
            return Ok(new { ClientId = clientId, PetCount = count });
        }
    }
}