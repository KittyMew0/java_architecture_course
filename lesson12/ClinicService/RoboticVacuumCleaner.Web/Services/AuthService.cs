using System.Net.Http.Json;
using RobotCleaner.Web.Models;
using RoboticVacuumCleaner.Web.Services.Interfaces;

namespace RoboticVacuumCleaner.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<AuthResult> RegisterAsync(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
            var result = await response.Content.ReadFromJsonAsync<AuthResult>();
            return result ?? new AuthResult { Success = false, Message = "Registration failed" };
        }

        public async Task<AuthResult> LoginAsync(LoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);
            var result = await response.Content.ReadFromJsonAsync<AuthResult>();

            if (result != null && result.Success && !string.IsNullOrEmpty(result.Token))
            {
                await _localStorage.SetItemAsync("authToken", result.Token);
                await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);
            }

            return result ?? new AuthResult { Success = false, Message = "Login failed" };
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("api/auth/me");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserDto>() ?? new UserDto();
                }
            }
            return new UserDto();
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            return !string.IsNullOrEmpty(token);
        }
    }
}