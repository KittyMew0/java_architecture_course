using System.ComponentModel.DataAnnotations;
using RoboticVacuumCleaner.Server.Models.Enums;

namespace RoboticVacuumCleaner.Server.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        [Required]
        public int RobotId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public RoomType Type { get; set; } = RoomType.LivingRoom;
        public double Area { get; set; }
        public FloorType FloorType { get; set; } = FloorType.Hardwood;
        public int CleaningIntensity { get; set; } = 5;
        public int WaterAmount { get; set; } = 50;
        public int CleaningPasses { get; set; } = 1;
        public int OrderPriority { get; set; } = 0;
        public bool IsEnabled { get; set; } = true;
        public string? Coordinates { get; set; } 
        public string? NoGoZones { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public virtual Robot Robot { get; set; } = null!;
        public virtual ICollection<CleaningSession> CleaningSessions { get; set; } = new List<CleaningSession>();
    }
}