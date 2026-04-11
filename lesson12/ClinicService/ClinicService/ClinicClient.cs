using ClinicService.Models;
using System.Text;
using System.Text.Json;

namespace ClinicService
{
    public class ClinicClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ClinicClient(string baseUrl, HttpClient httpClient)
        {
            _baseUrl = baseUrl;
            _httpClient = httpClient;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}api/Client/get-all");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Client>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Client>();
        }

        public async Task<Client?> GetClientByIdAsync(int clientId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}api/Client/get/{clientId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Client>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<int> CreateClientAsync(CreateClientRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}api/Client/create", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(result);
        }

        public async Task<int> UpdateClientAsync(UpdateClientRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}api/Client/update", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(result);
        }

        public async Task<int> DeleteClientAsync(int clientId)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}api/Client/delete?clientId={clientId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(result);
        }

        public async Task<List<Pet>> GetAllPetsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}api/Pet/get-all");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Pet>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Pet>();
        }

        public async Task<Pet?> GetPetByIdAsync(int petId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}api/Pet/get/{petId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Pet>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<int> CreatePetAsync(CreatePetRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}api/Pet/create", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(result);
        }

        public async Task<int> UpdatePetAsync(UpdatePetRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}api/Pet/update", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(result);
        }

        public async Task<int> DeletePetAsync(int petId)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}api/Pet/delete?petId={petId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(result);
        }
        public async Task<List<Consultation>> GetAllConsultationsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}api/Consultation/get-all");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Consultation>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Consultation>();
        }

        public async Task<Consultation?> GetConsultationByIdAsync(int consultationId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}api/Consultation/get/{consultationId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Consultation>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<int> CreateConsultationAsync(CreateConsultationRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}api/Consultation/create", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(result);
        }

        public async Task<int> UpdateConsultationAsync(UpdateConsultationRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}api/Consultation/update", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(result);
        }

        public async Task<int> DeleteConsultationAsync(int consultationId)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}api/Consultation/delete?consultationId={consultationId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(result);
        }
    }
}