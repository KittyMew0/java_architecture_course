using RoboticVacuumCleaner.Server.Models;
using RoboticVacuumCleaner.Server.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RoboticVacuumCleaner.Server.Models
{
    public class MaintenanceRecord
    {
        [Key]
        public int RecordId { get; set; }
        [Required]
        public int RobotId { get; set; }
        [Required]
        public MaintenanceType MaintenanceType { get; set; }
        public DateTime PerformedDate { get; set; } = DateTime.UtcNow;
        public DateTime? NextDueDate { get; set; }
        public string? ComponentName { get; set; }
        public string? ComponentSerial { get; set; }
        public string? Notes { get; set; }
        public string? PerformedBy { get; set; } 
        public bool IsCompleted { get; set; } = true;
        public bool ReminderSent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Robot Robot { get; set; } = null!;
    }
}