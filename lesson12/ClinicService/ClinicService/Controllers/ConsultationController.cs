using ClinicService.Models;
using ClinicService.Models.Requests;
using ClinicService.Models.Responses;
using ClinicService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IPetRepository _petRepository;

        public ConsultationController(
            IConsultationRepository consultationRepository,
            IClientRepository clientRepository,
            IPetRepository petRepository)
        {
            _consultationRepository = consultationRepository;
            _clientRepository = clientRepository;
            _petRepository = petRepository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateConsultationRequest createRequest)
        {
            if (createRequest == null)
                return BadRequest("Запрос не может быть пустым");

            var client = _clientRepository.GetById(createRequest.ClientId);
            if (client == null)
                return BadRequest($"Клиент с {createRequest.ClientId} идентификатором не найден");

            var pet = _petRepository.GetById(createRequest.PetId);
            if (pet == null)
                return BadRequest($"Питомец с {createRequest.PetId} идентификатором не найден");

            if (pet.ClientId != createRequest.ClientId)
                return BadRequest($"Питомец с {createRequest.PetId} идентификатором не принадлежит клиенту с {createRequest.ClientId} идентификатором");

            var consultation = new Consultation
            {
                ClientId = createRequest.ClientId,
                PetId = createRequest.PetId,
                ConsultationDate = createRequest.ConsultationDate,
                Description = createRequest.Description,
            };

            int res = _consultationRepository.Create(consultation);

            if (res > 0)
                return Ok(new { Message = "Консультация успешно добавлена ", Id = consultation.ConsultationId });

            return StatusCode(500, "Ошибка при создании консультации");
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdateConsultationRequest updateRequest)
        {
            if (updateRequest == null)
                return BadRequest("Запрос не может быть пустым");

            var existingConsultation = _consultationRepository.GetById(updateRequest.ConsultationId);
            if (existingConsultation == null)
                return NotFound($"Консультация с {updateRequest.ConsultationId} идентифиактором не найдена");

            var client = _clientRepository.GetById(updateRequest.ClientId);
            if (client == null)
                return BadRequest($"Клиент с {updateRequest.ClientId} идентификатором не найден");

            var pet = _petRepository.GetById(updateRequest.PetId);
            if (pet == null)
                return BadRequest($"Питомец с {updateRequest.PetId} идентификатором не найден");

            if (pet.ClientId != updateRequest.ClientId)
                return BadRequest($"Питомец с {updateRequest.PetId} идентификатором не принадлежит клиенту с идентификатором {updateRequest.ClientId}");

            var consultation = new Consultation
            {
                ConsultationId = updateRequest.ConsultationId,
                ClientId = updateRequest.ClientId,
                PetId = updateRequest.PetId,
                ConsultationDate = updateRequest.ConsultationDate,
                Description = updateRequest.Description,
            };

            int res = _consultationRepository.Update(consultation);

            if (res > 0)
                return Ok(new { Message = "Консультация обновлена успешно" });

            return StatusCode(500, "Ошибка при обновлении консультации");
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int consultationId)
        {
            var existingConsultation = _consultationRepository.GetById(consultationId);
            if (existingConsultation == null)
                return NotFound($"Консультация с {consultationId} идентификатором не найдена");

            int res = _consultationRepository.Delete(consultationId);

            if (res > 0)
                return Ok(new { Message = "Консультация успешно удалена" });

            return StatusCode(500, "Ошибка при удалении консультации");
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var consultations = _consultationRepository.GetAll();

            var response = consultations.Select(c => new ConsultationResponse
            {
                ConsultationId = c.ConsultationId,
                ClientId = c.ClientId,
                PetId = c.PetId,
                ConsultationDate = c.ConsultationDate,
                Description = c.Description
            }).ToList();

            return Ok(response);
        }

        [HttpGet("get/{consultationId}")]
        public IActionResult GetById([FromRoute] int consultationId)
        {
            var consultation = _consultationRepository.GetById(consultationId);

            if (consultation == null)
                return NotFound($"Консультация с {consultationId} идентификатором не найдена");

            var response = new ConsultationResponse
            {
                ConsultationId = consultation.ConsultationId,
                ClientId = consultation.ClientId,
                PetId = consultation.PetId,
                ConsultationDate = consultation.ConsultationDate,
                Description = consultation.Description
            };

            return Ok(response);
        }

        [HttpGet("get-by-client/{clientId}")]
        public IActionResult GetByClientId([FromRoute] int clientId)
        {
            var client = _clientRepository.GetById(clientId);
            if (client == null)
                return NotFound($"Клиент с {clientId} идентификатором не найден");

            var consultations = _consultationRepository.GetAll()
                .Where(c => c.ClientId == clientId)
                .ToList();

            var response = consultations.Select(c => new ConsultationResponse
            {
                ConsultationId = c.ConsultationId,
                ClientId = c.ClientId,
                PetId = c.PetId,
                ConsultationDate = c.ConsultationDate,
                Description = c.Description
            }).ToList();

            return Ok(response);
        }

        [HttpGet("get-by-pet/{petId}")]
        public IActionResult GetByPetId([FromRoute] int petId)
        {
            var pet = _petRepository.GetById(petId);
            if (pet == null)
                return NotFound($"Питомец с {petId} идентификатором не найден");

            var consultations = _consultationRepository.GetAll()
                .Where(c => c.PetId == petId)
                .ToList();

            var response = consultations.Select(c => new ConsultationResponse
            {
                ConsultationId = c.ConsultationId,
                ClientId = c.ClientId,
                PetId = c.PetId,
                ConsultationDate = c.ConsultationDate,
                Description = c.Description
            }).ToList();

            return Ok(response);
        }

        [HttpGet("upcoming")]
        public IActionResult GetUpcoming()
        {
            var consultations = _consultationRepository.GetUpcomingConsultations();
            return Ok(consultations);
        }

        [HttpGet("by-date-range")]
        public IActionResult GetByDateRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var consultations = _consultationRepository.GetByDateRange(from, to);
            return Ok(consultations);
        }

        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var stats = _consultationRepository.GetConsultationStatsByClient();
            return Ok(stats);
        }

        [HttpGet("check-consultation")]
        public IActionResult HasConsultationInPeriod(
            [FromQuery] int petId,
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var hasConsultation = _consultationRepository.HasConsultationInPeriod(petId, from, to);
            return Ok(new { PetId = petId, HasConsultation = hasConsultation });
        }
    }
}