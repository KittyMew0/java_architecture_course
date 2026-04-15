using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RoboticVacuumCleaner.Server.Controllers;
using RoboticVacuumCleaner.Server.Data;
using RoboticVacuumCleaner.Server.Models;
using System.Security.Claims;
using FluentAssertions;
using Xunit;

namespace RoboticVacuumCleaner.Tests.Controllers
{
    public class RobotsControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly RobotsController _controller;
        private readonly int _testUserId = 1;

        public RobotsControllerTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            _controller = new RobotsController(_context, Mock.Of<ILogger<RobotsController>>());

            // Setup mock user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetRobots_ShouldReturnUserRobots()
        {
            // Arrange
            var robot1 = new Robot
            {
                UserId = _testUserId,
                SerialNumber = "SN001",
                MacAddress = "AA:BB:CC:DD:EE:01",
                Model = "Model X1",
                Name = "Robot 1"
            };
            var robot2 = new Robot
            {
                UserId = _testUserId,
                SerialNumber = "SN002",
                MacAddress = "AA:BB:CC:DD:EE:02",
                Model = "Model X2",
                Name = "Robot 2"
            };
            var otherUserRobot = new Robot
            {
                UserId = 999,
                SerialNumber = "SN999",
                MacAddress = "AA:BB:CC:DD:EE:99",
                Model = "Model X3",
                Name = "Other Robot"
            };

            _context.Robots.AddRange(robot1, robot2, otherUserRobot);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetRobots();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var robots = okResult.Value.Should().BeAssignableTo<List<Robot>>().Subject;
            robots.Should().HaveCount(2);
            robots.Should().NotContain(r => r.UserId == 999);
        }

        [Fact]
        public async Task GetRobot_WithValidId_ShouldReturnRobot()
        {
            // Arrange
            var robot = new Robot
            {
                UserId = _testUserId,
                SerialNumber = "SN001",
                MacAddress = "AA:BB:CC:DD:EE:01",
                Model = "Model X1",
                Name = "Test Robot"
            };
            _context.Robots.Add(robot);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetRobot(robot.RobotId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedRobot = okResult.Value.Should().BeOfType<Robot>().Subject;
            returnedRobot.RobotId.Should().Be(robot.RobotId);
            returnedRobot.Name.Should().Be("Test Robot");
        }

        [Fact]
        public async Task GetRobot_WithInvalidId_ShouldReturnNotFound()
        {
            // Act
            var result = await _controller.GetRobot(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateRobot_WithValidData_ShouldCreateRobot()
        {
            // Arrange
            var robot = new Robot
            {
                SerialNumber = "SN001",
                MacAddress = "AA:BB:CC:DD:EE:01",
                Model = "Model X1",
                Name = "New Robot"
            };

            // Act
            var result = await _controller.CreateRobot(robot);

            // Assert
            var createdAtResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var createdRobot = createdAtResult.Value.Should().BeOfType<Robot>().Subject;
            createdRobot.RobotId.Should().BeGreaterThan(0);
            createdRobot.UserId.Should().Be(_testUserId);
        }

        [Fact]
        public async Task UpdateRobot_WithValidData_ShouldUpdateRobot()
        {
            // Arrange
            var robot = new Robot
            {
                UserId = _testUserId,
                SerialNumber = "SN001",
                MacAddress = "AA:BB:CC:DD:EE:01",
                Model = "Model X1",
                Name = "Original Name"
            };
            _context.Robots.Add(robot);
            await _context.SaveChangesAsync();

            robot.Name = "Updated Name";
            robot.BatteryLevel = 75;
            robot.Status = RobotStatus.Charging;

            // Act
            var result = await _controller.UpdateRobot(robot.RobotId, robot);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var updatedRobot = okResult.Value.Should().BeOfType<Robot>().Subject;
            updatedRobot.Name.Should().Be("Updated Name");
            updatedRobot.BatteryLevel.Should().Be(75);
        }

        [Fact]
        public async Task DeleteRobot_WithValidId_ShouldDeleteRobot()
        {
            // Arrange
            var robot = new Robot
            {
                UserId = _testUserId,
                SerialNumber = "SN001",
                MacAddress = "AA:BB:CC:DD:EE:01",
                Model = "Model X1",
                Name = "Robot to Delete"
            };
            _context.Robots.Add(robot);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteRobot(robot.RobotId);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            var deletedRobot = await _context.Robots.FindAsync(robot.RobotId);
            deletedRobot.Should().BeNull();
        }

        [Fact]
        public async Task StartCleaning_WithValidRobot_ShouldStartCleaning()
        {
            // Arrange
            var robot = new Robot
            {
                UserId = _testUserId,
                SerialNumber = "SN001",
                MacAddress = "AA:BB:CC:DD:EE:01",
                Model = "Model X1",
                Name = "Test Robot",
                Status = RobotStatus.Idle,
                BatteryLevel = 80
            };
            _context.Robots.Add(robot);
            await _context.SaveChangesAsync();

            var request = new StartCleaningRequest
            {
                Mode = CleaningMode.Turbo,
                WaterLevel = 70,
                SuctionPower = 8,
                RoomIds = new List<int> { 1, 2 }
            };

            // Act
            var result = await _controller.StartCleaning(robot.RobotId, request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            robot.Status.Should().Be(RobotStatus.Cleaning);

            var sessions = await _context.CleaningSessions.ToListAsync();
            sessions.Should().HaveCount(1);
            sessions[0].RobotId.Should().Be(robot.RobotId);
            sessions[0].Mode.Should().Be(CleaningMode.Turbo);
        }

        [Fact]
        public async Task StopCleaning_WithValidRobot_ShouldStopCleaning()
        {
            // Arrange
            var robot = new Robot
            {
                UserId = _testUserId,
                SerialNumber = "SN001",
                MacAddress = "AA:BB:CC:DD:EE:01",
                Model = "Model X1",
                Name = "Test Robot",
                Status = RobotStatus.Cleaning
            };
            _context.Robots.Add(robot);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.StopCleaning(robot.RobotId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            robot.Status.Should().Be(RobotStatus.Idle);
        }

        [Fact]
        public async Task ReturnToDock_WithValidRobot_ShouldStartReturn()
        {
            // Arrange
            var robot = new Robot
            {
                UserId = _testUserId,
                SerialNumber = "SN001",
                MacAddress = "AA:BB:CC:DD:EE:01",
                Model = "Model X1",
                Name = "Test Robot",
                Status = RobotStatus.Cleaning
            };
            _context.Robots.Add(robot);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.ReturnToDock(robot.RobotId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            robot.Status.Should().Be(RobotStatus.Returning);
        }
    }
}