using RoboticVacuumCleaner.Server.Models;
using RoboticVacuumCleaner.Server.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RoboticVacuumCleaner.Server.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        [Required]
        public int RobotId { get; set; }
        [Required]
        public string CronExpression { get; set; } = string.Empty;
        public TimeSpan Time { get; set; }
        public string DaysOfWeek { get; set; } = string.Empty; 
        public string? SpecificDates { get; set; } 
        public string Timezone { get; set; } = "Europe/Moscow";

        public string RoomsToClean { get; set; } = string.Empty; 
        public CleaningMode CleaningMode { get; set; } = CleaningMode.Normal;
        public bool IsEnabled { get; set; } = true;
        public bool IsRecurring { get; set; } = true;
        public DateTime? EndDate { get; set; }
        public string? CreatedBy { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModified { get; set; }
        // Navigation properties
        public virtual Robot Robot { get; set; } = null!;
        public virtual ICollection<CleaningSession> CleaningSessions { get; set; } = new List<CleaningSession>();
    }
}