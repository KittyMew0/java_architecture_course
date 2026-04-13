using RoboticVacuumCleaner.Server.DTOs;

namespace RoboticVacuumCleaner.Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<AuthResponse> ChangePasswordAsync(int userId, ChangePasswordRequest request);
        Task<bool> LogoutAsync(int userId, string refreshToken);
        Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request);
        UserDto? GetUserById(int userId);
    }
}
