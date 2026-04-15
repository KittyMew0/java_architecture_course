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

namespace RoboticVacuumCleaner.Tests.Components.Robots
{
    public class RobotsComponentTests : TestContext
    {
        private readonly Mock<IRobotService> _mockRobotService;

        public RobotsComponentTests()
        {
            _mockRobotService = new Mock<IRobotService>();
            Services.AddScoped(_ => _mockRobotService.Object);

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(x => x.IsAuthenticatedAsync()).ReturnsAsync(true);
            Services.AddScoped(_ => mockAuthService.Object);
        }

        [Fact]
        public async Task RobotsList_ShouldDisplayRobots()
        {
            // Arrange
            var robots = new List<Robot>
            {
                new Robot
                {
                    RobotId = 1,
                    Name = "Гостиная",
                    Model = "X1 Pro",
                    SerialNumber = "SN001",
                    BatteryLevel = 80,
                    FirmwareVersion = "1.0.0"
                },
                new Robot
                {
                    RobotId = 2,
                    Name = "Кухня",
                    Model = "X2",
                    SerialNumber = "SN002",
                    BatteryLevel = 45,
                    FirmwareVersion = "1.2.0"
                }
            };

            _mockRobotService.Setup(x => x.GetRobotsAsync()).ReturnsAsync(robots);

            // Act
            var cut = RenderComponent<Robots>();
            await Task.Delay(100);
            cut.Render();

            // Assert
            cut.FindAll(".robot-item").Count.Should().Be(2);
            cut.Find(".robot-name").TextContent.Should().Contain("Гостиная");
        }

        [Fact]
        public async Task AddRobotButton_Click_ShouldOpenModal()
        {
            // Arrange
            _mockRobotService.Setup(x => x.GetRobotsAsync()).ReturnsAsync(new List<Robot>());

            var cut = RenderComponent<Robots>();
            await Task.Delay(100);
            cut.Render();

            // Act
            var addButton = cut.Find(".page-header .btn-primary");
            addButton.Click();

            // Assert
            cut.Find(".modal-overlay").Should().NotBeNull();
            cut.Find(".modal-header h2").TextContent.Should().Contain("Добавить робота");
        }

        [Fact]
        public async Task AddRobot_WithValidData_ShouldCallCreateRobot()
        {
            // Arrange
            _mockRobotService.Setup(x => x.GetRobotsAsync()).ReturnsAsync(new List<Robot>());
            _mockRobotService.Setup(x => x.CreateRobotAsync(It.IsAny<Robot>()))
                .ReturnsAsync(new Robot { RobotId = 3, Name = "Новый робот" });

            var cut = RenderComponent<Robots>();
            await Task.Delay(100);
            cut.Render();

            // Open modal
            cut.Find(".page-header .btn-primary").Click();

            // Act - Fill form
            cut.Find(".modal-body input").Change("Новый робот");
            cut.Find(".modal-footer .btn-primary").Click();

            await Task.Delay(100);

            // Assert
            _mockRobotService.Verify(x => x.CreateRobotAsync(It.Is<Robot>(r => r.Name == "Новый робот")), Times.Once);
        }

        [Fact]
        public async Task DeleteRobot_ShouldConfirmAndDelete()
        {
            // Arrange
            var robots = new List<Robot>
            {
                new Robot { RobotId = 1, Name = "Робот для удаления", Model = "X1", SerialNumber = "SN001" }
            };

            _mockRobotService.Setup(x => x.GetRobotsAsync()).ReturnsAsync(robots);
            _mockRobotService.Setup(x => x.DeleteRobotAsync(1)).ReturnsAsync(true);

            var cut = RenderComponent<Robots>();
            await Task.Delay(100);
            cut.Render();

            // Act
            var deleteButton = cut.Find(".robot-actions .btn-icon.danger");
            deleteButton.Click();

        }
    }
}