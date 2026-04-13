using RoboticVacuumCleaner.Web.Models.Enums;

namespace RoboticVacuumCleaner.Web.Models
{
    public class StartCleaningRequest
    {
        public CleaningMode Mode { get; set; } = CleaningMode.Normal;
        public int WaterLevel { get; set; } = 50;
        public int SuctionPower { get; set; } = 5;
        public List<int> RoomIds { get; set; } = new();
    }
}
