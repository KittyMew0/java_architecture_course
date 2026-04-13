using RoboticVacuumCleaner.Web.Models.Enums;

namespace RoboticVacuumCleaner.Web.Models
{
    public class CleaningSession
    {
        public int SessionId { get; set; }
        public int RobotId { get; set; }
        public int? ScheduleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? DurationSeconds { get; set; }
        public CleaningMode Mode { get; set; }
        public int WaterLevel { get; set; }
        public int SuctionPower { get; set; }
        public double CleanedArea { get; set; }
        public List<int> RoomsCleaned { get; set; } = new();
        public double DustAmount { get; set; }
        public double WaterUsed { get; set; }
        public int BatteryStart { get; set; }
        public int BatteryEnd { get; set; }
        public bool ErrorOccurred { get; set; }
        public string? ErrorDetails { get; set; }
    }
}
