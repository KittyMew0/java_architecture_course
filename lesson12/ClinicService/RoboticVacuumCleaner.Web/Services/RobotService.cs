using RoboticVacuumCleaner.Web.Models;
using RoboticVacuumCleaner.Web.Services.Interfaces;
using System.Net.Http.Json;

namespace RoboticVacuumCleaner.Web.Services
{
    public class RobotService : IRobotService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public RobotService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private async Task SetAuthHeader()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<Robot>> GetRobotsAsync()
        {
            await SetAuthHeader();
            return await _httpClient.GetFromJsonAsync<List<Robot>>("api/robots") ?? new();
        }

        public async Task<Robot?> GetRobotAsync(int id)
        {
            await SetAuthHeader();
            return await _httpClient.GetFromJsonAsync<Robot>($"api/robots/{id}");
        }

        public async Task<Robot> CreateRobotAsync(Robot robot)
        {
            await SetAuthHeader();
            var response = await _httpClient.PostAsJsonAsync("api/robots", robot);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Robot>() ?? robot;
        }

        public async Task<Robot> UpdateRobotAsync(Robot robot)
        {
            await SetAuthHeader();
            var response = await _httpClient.PutAsJsonAsync($"api/robots/{robot.RobotId}", robot);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Robot>() ?? robot;
        }

        public async Task<bool> DeleteRobotAsync(int id)
        {
            await SetAuthHeader();
            var response = await _httpClient.DeleteAsync($"api/robots/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> StartCleaningAsync(int robotId, StartCleaningRequest? request = null)
        {
            await SetAuthHeader();
            request ??= new StartCleaningRequest();
            var response = await _httpClient.PostAsJsonAsync($"api/robots/{robotId}/start-cleaning", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> StopCleaningAsync(int robotId)
        {
            await SetAuthHeader();
            var response = await _httpClient.PostAsync($"api/robots/{robotId}/stop-cleaning", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ReturnToDockAsync(int robotId)
        {
            await SetAuthHeader();
            var response = await _httpClient.PostAsync($"api/robots/{robotId}/return-to-dock", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CleaningSession>> GetCleaningHistoryAsync(int robotId = 0)
        {
            await SetAuthHeader();
            var url = robotId > 0 ? $"api/cleaning/history?robotId={robotId}" : "api/cleaning/history";
            return await _httpClient.GetFromJsonAsync<List<CleaningSession>>(url) ?? new();
        }
    }
}
