using Microsoft.JSInterop;
using RoboticVacuumCleaner.Web.Services.Interfaces;
using System.Text.Json;

namespace RoboticVacuumCleaner.Web.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<T?> GetItemAsync<T>(string key)
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
                if (string.IsNullOrEmpty(json))
                    return default;

                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        }

        public async Task RemoveItemAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public async Task ClearAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.clear");
        }

        public async Task<bool> ContainsKeyAsync(string key)
        {
            var item = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            return !string.IsNullOrEmpty(item);
        }
    }
}
