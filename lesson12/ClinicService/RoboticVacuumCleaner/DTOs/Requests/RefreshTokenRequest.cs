using System.ComponentModel.DataAnnotations;

namespace RoboticVacuumCleaner.Server.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
