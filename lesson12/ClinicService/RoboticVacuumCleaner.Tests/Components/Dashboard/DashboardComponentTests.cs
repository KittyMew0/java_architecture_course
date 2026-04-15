using Bunit;
using FluentAssertions.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RoboticVacuumCleaner.Tests.Helpers;
using RoboticVacuumCleaner.Web.Pages;
using RoboticVacuumCleaner.Web.Services;

namespace RoboticVacuumCleaner.Tests.Components.Dashboard
{
    public class DashboardComponentTests : TestContext
    {
        private readonly Mock<IRobotService> _mockRobotService;

        public DashboardComponentTests()
        {
            _mockRobotService = new Mock<IRobotService>();
            Services.AddScoped(_ => _mockRobotService.Object);
            AddMockAuthService();
            AddMockLocalStorage();
        }

        private void AddMockAuthService()
        {
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(x => x.IsAuthenticatedAsync()).ReturnsAsync(true);
            Services.AddScoped(_ => mockAuthService.Object);
        }

        private void AddMockLocalStorage()
        {
            var mockLocalStorage = new Mock<ILocalStorageService>();
            Services.AddScoped(_ => mockLocalStorage.Object);
        }

        [Fact]
        public async Task Dashboard_ShouldLoadAndDisplayRobots()
        {
            // Arrange
            var robots = new List<Robot>
            {
                new Robot { RobotId = 1, Name = "Робот 1", Status = RobotStatus.Idle, BatteryLevel = 80 },
                new Robot { RobotId = 2, Name = "Робот 2", Status = RobotStatus.Cleaning, BatteryLevel = 45 }
            };

            _mockRobotService.Setup(x => x.GetRobotsAsync()).ReturnsAsync(robots);
            _mockRobotService.Setup(x => x.GetCleaningHistoryAsync(0)).ReturnsAsync(new List<CleaningSession>());

            // Act
            var cut = RenderComponent<Dashboard>();

            // Wait for data to load
            await Task.Delay(100);
            cut.Render();

            // Assert
            cut.FindAll(".robot-card").Count.Should().Be(2);
            cut.Find(".stat-value").TextContent.Should().Contain("2");
        }

        [Fact]
        public async Task Dashboard_StartCleaning_ShouldCallRobotService()
        {
            // Arrange
            var robot = new Robot { RobotId = 1, Name = "Робот 1", Status = RobotStatus.Idle, BatteryLevel = 80 };

            _mockRobotService.Setup(x => x.GetRobotsAsync()).ReturnsAsync(new List<Robot> { robot });
            _mockRobotService.Setup(x => x.GetCleaningHistoryAsync(0)).ReturnsAsync(new List<CleaningSession>());

            var cut = RenderComponent<Dashboard>();
            await Task.Delay(100);
            cut.Render();

            // Act
            var startButton = cut.Find(".robot-actions .btn-success");
            startButton.Click();

            // Assert
            _mockRobotService.Verify(x => x.StartCleaningAsync(1, It.IsAny<StartCleaningRequest>()), Times.Once);
        }

        [Fact]
        public async Task Dashboard_StopCleaning_ShouldCallRobotService()
        {
            // Arrange
            var robot = new Robot { RobotId = 1, Name = "Робот 1", Status = RobotStatus.Cleaning, BatteryLevel = 80 };

            _mockRobotService.Setup(x => x.GetRobotsAsync()).ReturnsAsync(new List<Robot> { robot });
            _mockRobotService.Setup(x => x.GetCleaningHistoryAsync(0)).ReturnsAsync(new List<CleaningSession>());

            var cut = RenderComponent<Dashboard>();
            await Task.Delay(100);
            cut.Render();

            // Act
            var stopButton = cut.Find(".robot-actions .btn-warning");
            stopButton.Click();

            // Assert
            _mockRobotService.Verify(x => x.StopCleaningAsync(1), Times.Once);
        }

        [Fact]
        public async Task Dashboard_WithNoRobots_ShouldShowEmptyState()
        {
            // Arrange
            _mockRobotService.Setup(x => x.GetRobotsAsync()).ReturnsAsync(new List<Robot>());
            _mockRobotService.Setup(x => x.GetCleaningHistoryAsync(0)).ReturnsAsync(new List<CleaningSession>());

            // Act
            var cut = RenderComponent<Dashboard>();
            await Task.Delay(100);
            cut.Render();

            // Assert
            cut.Find(".empty-state").Should().NotBeNull();
            cut.Find(".empty-state h3").TextContent.Should().Contain("нет роботов");
            cut.Find(".btn-primary").TextContent.Should().Contain("Добавить робота");
        }

        [Fact]
        public async Task Dashboard_ShouldDisplayBatteryLevelCorrectly()
        {
            // Arrange
            var robots = new List<Robot>
            {
                new Robot { RobotId = 1, Name = "Робот 1", Status = RobotStatus.Idle, BatteryLevel = 75 }
            };

            _mockRobotService.Setup(x => x.GetRobotsAsync()).ReturnsAsync(robots);
            _mockRobotService.Setup(x => x.GetCleaningHistoryAsync(0)).ReturnsAsync(new List<CleaningSession>());

            // Act
            var cut = RenderComponent<Dashboard>();
            await Task.Delay(100);
            cut.Render();

            // Assert
            var batteryIndicator = cut.Find(".battery-level");
            batteryIndicator.GetAttribute("style").Should().Contain("width: 75%");
            cut.Find(".battery-indicator span").TextContent.Should().Contain("75%");
        }
    }
}