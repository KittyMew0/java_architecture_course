using RoboticVacuumCleaner.Server.DTOs;
using RoboticVacuumCleaner.Server.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoboticVacuumCleaner.Server.Data;
using System.IdentityModel.Tokens.Jwt;
using RoboticVacuumCleaner.Server.Services.Interfaces;
using RoboticVacuumCleaner.Server.DTOs.Requests;

namespace RoboticVacuumCleaner.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            ApplicationDbContext context,
            IConfiguration configuration,
            IEmailService emailService,
            ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (existingUser != null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Пользователь с таким email уже существует"
                    };
                }

                var user = new User
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash = HashPassword(request.Password),
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmationToken = GenerateEmailConfirmationToken()
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await _emailService.SendEmailConfirmationAsync(user.Email, user.EmailConfirmationToken!);

                var token = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _context.SaveChangesAsync();

                return new AuthResponse
                {
                    Success = true,
                    Message = "Регистрация успешна. Пожалуйста, подтвердите email.",
                    Token = token,
                    RefreshToken = refreshToken,
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Ошибка при регистрации. Попробуйте позже."
                };
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Неверный email или пароль"
                    };
                }

                if (!user.IsEmailConfirmed)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Пожалуйста, подтвердите email перед входом"
                    };
                }

                user.LastLoginAt = DateTime.UtcNow;

                var token = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _context.SaveChangesAsync();

                return new AuthResponse
                {
                    Success = true,
                    Message = "Вход выполнен успешно",
                    Token = token,
                    RefreshToken = refreshToken,
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Ошибка при входе. Попробуйте позже."
                };
            }
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid refresh token"
                };
            }

            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Success = true,
                Token = newToken,
                RefreshToken = newRefreshToken,
                User = MapToUserDto(user)
            };
        }

        public async Task<AuthResponse> ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || !VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Неверный текущий пароль"
                };
            }

            user.PasswordHash = HashPassword(request.NewPassword);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Success = true,
                Message = "Пароль успешно изменен"
            };
        }

        public async Task<bool> LogoutAsync(int userId, string refreshToken)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user != null && user.RefreshToken == refreshToken)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return new AuthResponse
                {
                    Success = true,
                    Message = "Если пользователь существует, инструкции отправлены на email"
                };
            }

            var resetToken = GeneratePasswordResetToken();
            user.EmailConfirmationToken = resetToken;
            await _context.SaveChangesAsync();

            await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken);

            return new AuthResponse
            {
                Success = true,
                Message = "Инструкции по сбросу пароля отправлены на email"
            };
        }

        public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.EmailConfirmationToken == request.Token);

            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Недействительная ссылка сброса пароля"
                };
            }

            user.PasswordHash = HashPassword(request.NewPassword);
            user.EmailConfirmationToken = null;
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Success = true,
                Message = "Пароль успешно сброшен"
            };
        }

        public UserDto? GetUserById(int userId)
        {
            var user = _context.Users.Find(userId);
            return user != null ? MapToUserDto(user) : null;
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"] ?? "your-secret-key-min-32-chars-long!");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateEmailConfirmationToken()
        {
            return Guid.NewGuid().ToString();
        }

        private string GeneratePasswordResetToken()
        {
            return Guid.NewGuid().ToString();
        }

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
                Language = user.Language,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
