using RoboticVacuumCleaner.Server.Models;
using System.ComponentModel.DataAnnotations;
using RoboticVacuumCleaner.Server.Models.Enums;

namespace RoboticVacuumCleaner.Server.Models
{
    public class Robot
    {
        [Key]
        public int RobotId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string SerialNumber { get; set; } = string.Empty;
        [Required]
        public string MacAddress { get; set; } = string.Empty;
        [Required]
        public string Model { get; set; } = string.Empty;
        public string Name { get; set; } = "Мой робот-пылесос";
        public RobotStatus Status { get; set; } = RobotStatus.Offline;
        public int BatteryLevel { get; set; } = 0;
        public bool IsDocked { get; set; } = true;
        public string? CurrentActivity { get; set; }
        public int? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string FirmwareVersion { get; set; } = "1.0.0";
        public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
        public virtual ICollection<CleaningSession> CleaningSessions { get; set; } = new List<CleaningSession>();
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
    }
}