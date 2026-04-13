using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoboticVacuumCleaner.Server.Data;
using RoboticVacuumCleaner.Server.Models;
using RoboticVacuumCleaner.Server.Models.Enums;
using System.Security.Claims;

namespace RoboticVacuumCleaner.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RobotsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RobotsController> _logger;

        public RobotsController(ApplicationDbContext context, ILogger<RobotsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRobots()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var robots = await _context.Robots
                .Where(r => r.UserId == userId)
                .Include(r => r.Rooms)
                .ToListAsync();

            return Ok(robots);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRobot(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var robot = await _context.Robots
                .Include(r => r.Rooms)
                .FirstOrDefaultAsync(r => r.RobotId == id && r.UserId == userId);

            if (robot == null)
                return NotFound();

            return Ok(robot);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRobot([FromBody] Robot robot)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            robot.UserId = userId;
            robot.CreatedAt = DateTime.UtcNow;

            _context.Robots.Add(robot);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRobot), new { id = robot.RobotId }, robot);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRobot(int id, [FromBody] Robot robot)
        {
            if (id != robot.RobotId)
                return BadRequest();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var existingRobot = await _context.Robots
                .FirstOrDefaultAsync(r => r.RobotId == id && r.UserId == userId);

            if (existingRobot == null)
                return NotFound();

            existingRobot.Name = robot.Name;
            existingRobot.Status = robot.Status;
            existingRobot.BatteryLevel = robot.BatteryLevel;
            existingRobot.IsDocked = robot.IsDocked;
            existingRobot.CurrentActivity = robot.CurrentActivity;
            existingRobot.LastSeenAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(existingRobot);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRobot(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var robot = await _context.Robots
                .FirstOrDefaultAsync(r => r.RobotId == id && r.UserId == userId);

            if (robot == null)
                return NotFound();

            _context.Robots.Remove(robot);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/start-cleaning")]
        public async Task<IActionResult> StartCleaning(int id, [FromBody] StartCleaningRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var robot = await _context.Robots
                .FirstOrDefaultAsync(r => r.RobotId == id && r.UserId == userId);

            if (robot == null)
                return NotFound();

            // Start cleaning session
            var session = new CleaningSession
            {
                RobotId = id,
                StartTime = DateTime.UtcNow,
                Mode = request.Mode,
                WaterLevel = request.WaterLevel,
                SuctionPower = request.SuctionPower,
                RoomsCleaned = System.Text.Json.JsonSerializer.Serialize(request.RoomIds),
                BatteryStart = robot.BatteryLevel
            };

            _context.CleaningSessions.Add(session);

            robot.Status = RobotStatus.Cleaning;
            robot.CurrentActivity = "Cleaning";
            robot.LastSeenAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { SessionId = session.SessionId, Message = "Cleaning started" });
        }

        [HttpPost("{id}/stop-cleaning")]
        public async Task<IActionResult> StopCleaning(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var robot = await _context.Robots
                .FirstOrDefaultAsync(r => r.RobotId == id && r.UserId == userId);

            if (robot == null)
                return NotFound();

            robot.Status = RobotStatus.Idle;
            robot.CurrentActivity = "Idle";
            robot.LastSeenAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Cleaning stopped" });
        }

        [HttpPost("{id}/return-to-dock")]
        public async Task<IActionResult> ReturnToDock(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var robot = await _context.Robots
                .FirstOrDefaultAsync(r => r.RobotId == id && r.UserId == userId);

            if (robot == null)
                return NotFound();

            robot.Status = RobotStatus.Returning;
            robot.CurrentActivity = "Returning to dock";
            robot.LastSeenAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Returning to dock" });
        }
    }
}