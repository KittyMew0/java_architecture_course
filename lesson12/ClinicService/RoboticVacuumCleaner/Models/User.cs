using System.ComponentModel.DataAnnotations;
using RoboticVacuumCleaner.Server.Models.Enums;

namespace RoboticVacuumCleaner.Server.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public string Language { get; set; } = "ru-RU";
        public string Timezone { get; set; } = "Europe/Moscow";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<Robot> Robots { get; set; } = new List<Robot>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}