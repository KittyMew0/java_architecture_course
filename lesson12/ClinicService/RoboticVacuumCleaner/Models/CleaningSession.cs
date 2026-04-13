using System.ComponentModel.DataAnnotations;
using RoboticVacuumCleaner.Server.Models.Enums;

namespace RoboticVacuumCleaner.Server.Models
{
    public class CleaningSession
    {
        [Key]
        public int SessionId { get; set; }
        [Required]
        public int RobotId { get; set; }
        public int? ScheduleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? DurationSeconds { get; set; }
        public CleaningMode Mode { get; set; } = CleaningMode.Normal;
        public int WaterLevel { get; set; } = 50;
        public int SuctionPower { get; set; } = 5;
        public double CleanedArea { get; set; }
        public string? RoomsCleaned { get; set; }
        public double DustAmount { get; set; } 
        public double WaterUsed { get; set; } 
        public int BatteryStart { get; set; }
        public int BatteryEnd { get; set; }
        public bool ErrorOccurred { get; set; }
        public string? ErrorDetails { get; set; }
        public string? CleaningMap { get; set; } 
        public string? PathHistory { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual Robot Robot { get; set; } = null!;
        public virtual Schedule? Schedule { get; set; }
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}