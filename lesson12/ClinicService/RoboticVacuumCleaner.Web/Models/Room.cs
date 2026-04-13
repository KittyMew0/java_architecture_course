using RoboticVacuumCleaner.Web.Models.Enums;

namespace RoboticVacuumCleaner.Web.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int RobotId { get; set; }
        public string Name { get; set; } = string.Empty;
        public RoomType Type { get; set; }
        public double Area { get; set; }
        public FloorType FloorType { get; set; }
        public int CleaningIntensity { get; set; } = 5;
        public int WaterAmount { get; set; } = 50;
        public int CleaningPasses { get; set; } = 1;
        public int OrderPriority { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
