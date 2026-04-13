namespace RoboticVacuumCleaner.Web.Models
{
    public class Robot
    {
        public int RobotId { get; set; }
        public int UserId { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Name { get; set; } = "Мой робот-пылесос";
        public RobotStatus Status { get; set; } = RobotStatus.Offline;
        public int BatteryLevel { get; set; }
        public bool IsDocked { get; set; }
        public string? CurrentActivity { get; set; }
        public int? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string FirmwareVersion { get; set; } = "1.0.0";
        public DateTime LastSeenAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Room> Rooms { get; set; } = new();
    }
}