using ClinicService.Models;
using ClinicService.Models.Requests;
using ClinicService.Models.Responses;
using ClinicService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ClinicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateClientRequest createRequest)
        {
            if (createRequest == null)
                return BadRequest("Запрос не может быть пустым");

            // Проверяем, не существует ли клиент с таким документом
            var existingClient = _clientRepository.GetByDocument(createRequest.Document);
            if (existingClient != null)
                return BadRequest($"Клиент с таким документом {createRequest.Document} уже существует");

            var client = new Client
            {
                Document = createRequest.Document,
                SurName = createRequest.SurName,
                FirstName = createRequest.FirstName,
                Patronymic = createRequest.Patronymic,
                Birthday = createRequest.Birthday,
            };

            int res = _clientRepository.Create(client);

            if (res > 0)
                return Ok(new { Message = "Клиент создан успешно", Id = client.ClientId });

            return StatusCode(500, "Ошибка при создании клиента");
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdateClientRequest updateRequest)
        {
            if (updateRequest == null)
                return BadRequest("Запрос не может быть пустым");

            var existingClient = _clientRepository.GetById(updateRequest.ClientId);
            if (existingClient == null)
                return NotFound($"Клиент с {updateRequest.ClientId} идентификатором не найден");

            var client = new Client
            {
                ClientId = updateRequest.ClientId,
                Document = updateRequest.Document,
                SurName = updateRequest.SurName,
                FirstName = updateRequest.FirstName,
                Patronymic = updateRequest.Patronymic,
                Birthday = updateRequest.Birthday,
            };

            int res = _clientRepository.Update(client);

            if (res > 0)
                return Ok(new { Message = "Данные о клиенте успешно обновлены" });

            return StatusCode(500, "Ошибка при обновлении данных клиента");
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int clientId)
        {
            var existingClient = _clientRepository.GetById(clientId);
            if (existingClient == null)
                return NotFound($"Клиент с {clientId} идентификатором не найден");

            int res = _clientRepository.Delete(clientId);

            if (res > 0)
                return Ok(new { Message = "Данные о клиенте удалены успешно" });

            return StatusCode(500, "Ошибка при удалении данных о клиенте");
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var clients = _clientRepository.GetAll();

            var response = clients.Select(c => new ClientResponse
            {
                ClientId = c.ClientId,
                Document = c.Document,
                SurName = c.SurName,
                FirstName = c.FirstName,
                Patronymic = c.Patronymic,
                Birthday = c.Birthday
            }).ToList();

            return Ok(response);
        }

        [HttpGet("get/{clientId}")]
        public IActionResult GetById([FromRoute] int clientId)
        {
            var client = _clientRepository.GetById(clientId);

            if (client == null)
                return NotFound($"Клиент с {clientId} идентификатором не найден");

            var response = new ClientResponse
            {
                ClientId = client.ClientId,
                Document = client.Document,
                SurName = client.SurName,
                FirstName = client.FirstName,
                Patronymic = client.Patronymic,
                Birthday = client.Birthday
            };

            return Ok(response);
        }

        [HttpGet("by-document/{document}")]
        public IActionResult GetByDocument([FromRoute] string document)
        {
            if (string.IsNullOrWhiteSpace(document))
                return BadRequest("Документ не может быть пустым");

            var client = _clientRepository.GetByDocument(document);

            if (client == null)
                return NotFound($"Клиент с {document} документом не найден");

            var response = new ClientResponse
            {
                ClientId = client.ClientId,
                Document = client.Document,
                SurName = client.SurName,
                FirstName = client.FirstName,
                Patronymic = client.Patronymic,
                Birthday = client.Birthday
            };

            return Ok(response);
        }

        [HttpGet("by-surname/{surName}")]
        public IActionResult GetBySurName([FromRoute] string surName)
        {
            if (string.IsNullOrWhiteSpace(surName))
                return BadRequest("Поле 'Фамилия' не может быть пустым");

            var clients = _clientRepository.GetBySurName(surName);

            var response = clients.Select(c => new ClientResponse
            {
                ClientId = c.ClientId,
                Document = c.Document,
                SurName = c.SurName,
                FirstName = c.FirstName,
                Patronymic = c.Patronymic,
                Birthday = c.Birthday
            }).ToList();

            return Ok(response);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return BadRequest("Поле запроса не может быть пустым");

            var clients = _clientRepository.GetBySurNameContains(searchText);

            var response = clients.Select(c => new ClientResponse
            {
                ClientId = c.ClientId,
                Document = c.Document,
                SurName = c.SurName,
                FirstName = c.FirstName,
                Patronymic = c.Patronymic,
                Birthday = c.Birthday
            }).ToList();

            return Ok(response);
        }

        [HttpGet("by-birthday-range")]
        public IActionResult GetByBirthdayRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from > to)
                return BadRequest("Поле 'В период с' не может быть больше чем 'по _ год'.");

            var clients = _clientRepository.GetByBirthdayRange(from, to);

            var response = clients.Select(c => new ClientResponse
            {
                ClientId = c.ClientId,
                Document = c.Document,
                SurName = c.SurName,
                FirstName = c.FirstName,
                Patronymic = c.Patronymic,
                Birthday = c.Birthday
            }).ToList();

            return Ok(response);
        }

        [HttpGet("get-all-with-pets")]
        public IActionResult GetAllWithPets()
        {
            var clients = _clientRepository.GetAllWithPets();
            return Ok(clients);
        }

        [HttpGet("exists/{clientId}")]
        public IActionResult Exists([FromRoute] int clientId)
        {
            var exists = _clientRepository.Exists(clientId);
            return Ok(new { ClientId = clientId, Exists = exists });
        }

        [HttpGet("by-age")]
        public IActionResult GetByAge([FromQuery] int minAge, [FromQuery] int maxAge)
        {
            if (minAge < 0 || maxAge < 0)
                return BadRequest("Возраст не может быть отрицательным");

            if (minAge > maxAge)
                return BadRequest("Минимальный возраст не может быть больше максимального");

            DateTime now = DateTime.Now;
            DateTime to = now.AddYears(-minAge);
            DateTime from = now.AddYears(-maxAge - 1);

            var clients = _clientRepository.GetByBirthdayRange(from, to);

            var response = clients.Select(c => new ClientResponse
            {
                ClientId = c.ClientId,
                Document = c.Document,
                SurName = c.SurName,
                FirstName = c.FirstName,
                Patronymic = c.Patronymic,
                Birthday = c.Birthday
            }).ToList();

            return Ok(new
            {
                MinAge = minAge,
                MaxAge = maxAge,
                Count = response.Count,
                Clients = response
            });
        }

        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            var allClients = _clientRepository.GetAll();
            var now = DateTime.Now;

            var stats = new
            {
                TotalClients = allClients.Count,
                AverageAge = allClients.Any() ? allClients.Average(c => (now - c.Birthday).TotalDays / 365.25) : 0,
                YoungestClient = allClients.Any() ? allClients.OrderByDescending(c => c.Birthday).First() : null,
                OldestClient = allClients.Any() ? allClients.OrderBy(c => c.Birthday).First() : null,
                ClientsByMonth = allClients
                    .GroupBy(c => c.Birthday.Month)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return Ok(stats);
        }

        [HttpPost("delete-bulk")]
        public IActionResult DeleteBulk([FromBody] List<int> clientIds)
        {
            if (clientIds == null || !clientIds.Any())
                return BadRequest("Идентификаторы клиентов не могут быть пустыми");

            var results = new List<object>();

            foreach (var clientId in clientIds)
            {
                try
                {
                    var client = _clientRepository.GetById(clientId);
                    if (client == null)
                    {
                        results.Add(new { ClientId = clientId, Success = false, Message = "Клиент не найден" });
                        continue;
                    }

                    int res = _clientRepository.Delete(clientId);
                    results.Add(new { ClientId = clientId, Success = res > 0 });
                }
                catch (Exception ex)
                {
                    results.Add(new { ClientId = clientId, Success = false, Message = ex.Message });
                }
            }

            return Ok(new
            {
                TotalRequested = clientIds.Count,
                Successful = results.Count(r => (bool)r.GetType().GetProperty("УСпешно")?.GetValue(r)),
                Results = results
            });
        }

        [HttpPatch("patch/{clientId}")]
        public IActionResult Patch([FromRoute] int clientId, [FromBody] JsonPatchDocument<Client> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest("Изменения не могут быть пустыми");

            var existingClient = _clientRepository.GetById(clientId);
            if (existingClient == null)
                return NotFound($"Клиент с {clientId} идентификатором не найден");

            patchDocument.ApplyTo(existingClient, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int res = _clientRepository.Update(existingClient);

            if (res > 0)
                return Ok(new { Message = "Данные о клиенте успешно обновлены" });

            return StatusCode(500, "Не удалось обновить данные о клиенте");
        }

        [HttpGet("export/csv")]
        public IActionResult ExportToCsv()
        {
            var clients = _clientRepository.GetAll();

            var csv = new StringBuilder();
            csv.AppendLine("ClientId,Document,SurName,FirstName,Patronymic,Birthday");

            foreach (var client in clients)
            {
                csv.AppendLine($"{client.ClientId},{client.Document},{client.SurName},{client.FirstName},{client.Patronymic},{client.Birthday:yyyy-MM-dd}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"clients_export_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        }

        [HttpGet("page")]
        public IActionResult GetPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var allClients = _clientRepository.GetAll();
            var totalCount = allClients.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var clients = allClients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPreviousPage = page > 1,
                HasNextPage = page < totalPages,
                Clients = clients.Select(c => new ClientResponse
                {
                    ClientId = c.ClientId,
                    Document = c.Document,
                    SurName = c.SurName,
                    FirstName = c.FirstName,
                    Patronymic = c.Patronymic,
                    Birthday = c.Birthday
                })
            };

            return Ok(response);
        }
    }
}