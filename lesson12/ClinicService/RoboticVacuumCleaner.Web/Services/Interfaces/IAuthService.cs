using RoboticVacuumCleaner.Web.Models;

namespace RoboticVacuumCleaner.Web.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterModel model);
        Task<AuthResult> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<UserDto> GetCurrentUserAsync();
        Task<bool> IsAuthenticatedAsync();
    }
}
