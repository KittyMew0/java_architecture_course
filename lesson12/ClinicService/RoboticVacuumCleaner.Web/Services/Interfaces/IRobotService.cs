using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace RoboticVacuumCleaner.Web.Services.Interfaces
{
    public interface IRobotService
    {
        Task<List<Robot>> GetRobotsAsync();
        Task<Robot?> GetRobotAsync(int id);
        Task<Robot> CreateRobotAsync(Robot robot);
        Task<Robot> UpdateRobotAsync(Robot robot);
        Task<bool> DeleteRobotAsync(int id);
        Task<bool> StartCleaningAsync(int robotId, StartCleaningRequest? request = null);
        Task<bool> StopCleaningAsync(int robotId);
        Task<bool> ReturnToDockAsync(int robotId);
        Task<List<CleaningSession>> GetCleaningHistoryAsync(int robotId = 0);
    }
}