using Bunit;
using FluentAssertions.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RoboticVacuumCleaner.Tests.Helpers;
using RoboticVacuumCleaner.Web.Pages;
using RoboticVacuumCleaner.Web.Services;
using Xunit;

namespace RoboticVacuumCleaner.Tests.Components.Schedule
{
    public class ScheduleComponentTests : TestContext
    {
        private readonly Mock<IRobotService> _mockRobotService;

        public ScheduleComponentTests()
        {
            _mockRobotService = new Mock<IRobotService>();
            Services.AddScoped(_ => _mockRobotService.Object);

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(x => x.IsAuthenticatedAsync()).ReturnsAsync(true);
            Services.AddScoped(_ => mockAuthService.Object);
        }

        [Fact]
        public async Task SchedulePage_ShouldDisplayCalendar()
        {
            // Arrange
            _mockRobotService.Setup(x => x.GetSchedulesAsync()).ReturnsAsync(new List<Schedule>());
            _mockRobotService.Setup(x => x.GetRoomsAsync()).ReturnsAsync(new List<Room>());

            // Act
            var cut = RenderComponent<Schedule>();
            await Task.Delay(100);
            cut.Render();

            // Assert
            cut.Find(".calendar-section").Should().NotBeNull();
            cut.Find(".calendar-weekdays").Should().NotBeNull();
            cut.Find(".calendar-days").Should().NotBeNull();
        }

        [Fact]
        public async Task CreateScheduleButton_Click_ShouldOpenModal()
        {
            // Arrange
            _mockRobotService.Setup(x => x.GetSchedulesAsync()).ReturnsAsync(new List<Schedule>());
            _mockRobotService.Setup(x => x.GetRoomsAsync()).ReturnsAsync(new List<Room>());

            var cut = RenderComponent<Schedule>();
            await Task.Delay(100);
            cut.Render();

            // Act
            var createButton = cut.Find(".page-header .btn-primary");
            createButton.Click();

            // Assert
            cut.Find(".modal-overlay").Should().NotBeNull();
            cut.Find(".modal-header h2").TextContent.Should().Contain("Создание расписания");
        }

        [Fact]
        public async Task ScheduleList_ShouldDisplayExistingSchedules()
        {
            // Arrange
            var schedules = new List<Schedule>
            {
                new Schedule
                {
                    ScheduleId = 1,
                    Time = new TimeSpan(10, 0, 0),
                    IsEnabled = true,
                    DaysOfWeek = "[1,2,3,4,5]",
                    RoomsToClean = "[1,2]"
                }
            };

            var rooms = new List<Room>
            {
                new Room { RoomId = 1, Name = "Гостиная" },
                new Room { RoomId = 2, Name = "Кухня" }
            };

            _mockRobotService.Setup(x => x.GetSchedulesAsync()).ReturnsAsync(schedules);
            _mockRobotService.Setup(x => x.GetRoomsAsync()).ReturnsAsync(rooms);

            // Act
            var cut = RenderComponent<Schedule>();
            await Task.Delay(100);
            cut.Render();

            // Assert
            cut.FindAll(".schedule-card").Count.Should().Be(1);
            cut.Find(".schedule-time span").TextContent.Should().Contain("10:00");
        }

        [Fact]
        public async Task ToggleSchedule_ShouldCallUpdateService()
        {
            // Arrange
            var schedule = new Schedule
            {
                ScheduleId = 1,
                Time = new TimeSpan(10, 0, 0),
                IsEnabled = true,
                DaysOfWeek = "[1,2,3]",
                RoomsToClean = "[1]"
            };

            _mockRobotService.Setup(x => x.GetSchedulesAsync()).ReturnsAsync(new List<Schedule> { schedule });
            _mockRobotService.Setup(x => x.GetRoomsAsync()).ReturnsAsync(new List<Room>());
            _mockRobotService.Setup(x => x.UpdateScheduleAsync(It.IsAny<Schedule>()))
                .ReturnsAsync(schedule);

            var cut = RenderComponent<Schedule>();
            await Task.Delay(100);
            cut.Render();

            // Act
            var toggle = cut.Find(".schedule-switch input");
            toggle.Change(true);

            // Assert
            _mockRobotService.Verify(x => x.UpdateScheduleAsync(It.Is<Schedule>(s => s.ScheduleId == 1)), Times.Once);
        }
    }
}