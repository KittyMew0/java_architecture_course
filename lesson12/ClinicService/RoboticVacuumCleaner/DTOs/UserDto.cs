namespace RoboticVacuumCleaner.Server.DTOs // Data Transfer Object between server and client
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public string Language { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
