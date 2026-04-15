using Bunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RoboticVacuumCleaner.Web.Services;

namespace RoboticVacuumCleaner.Tests.Helpers
{
    public static class ContextExtensionsTest
    {
        public static void AddMockAuthService(this TestContext ctx, bool isAuthenticated = true)
        {
            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(x => x.IsAuthenticatedAsync())
                .ReturnsAsync(isAuthenticated);

            if (isAuthenticated)
            {
                mockAuthService.Setup(x => x.GetCurrentUserAsync())
                    .ReturnsAsync(new UserDto
                    {
                        UserId = 1,
                        Email = "test@example.com",
                        FirstName = "Тест",
                        LastName = "Пользователь"
                    });
            }

            ctx.Services.AddScoped(_ => mockAuthService.Object);
        }

        public static void AddMockRobotService(this TestContext ctx)
        {
            var mockRobotService = new Mock<IRobotService>();

            mockRobotService.Setup(x => x.GetRobotsAsync())
                .ReturnsAsync(new List<Robot>
                {
                    new Robot
                    {
                        RobotId = 1,
                        Name = "Тестовый робот",
                        Model = "X1 Pro",
                        Status = RobotStatus.Idle,
                        BatteryLevel = 85,
                        FirmwareVersion = "1.0.0"
                    },
                    new Robot
                    {
                        RobotId = 2,
                        Name = "Кухонный робот",
                        Model = "X2",
                        Status = RobotStatus.Cleaning,
                        BatteryLevel = 45,
                        FirmwareVersion = "1.2.0"
                    }
                });

            mockRobotService.Setup(x => x.StartCleaningAsync(It.IsAny<int>(), It.IsAny<StartCleaningRequest>()))
                .ReturnsAsync(true);

            mockRobotService.Setup(x => x.StopCleaningAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            mockRobotService.Setup(x => x.ReturnToDockAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            ctx.Services.AddScoped(_ => mockRobotService.Object);
        }

        public static void AddMockLocalStorage(this TestContext ctx)
        {
            var mockLocalStorage = new Mock<ILocalStorageService>();

            mockLocalStorage.Setup(x => x.GetItemAsync<string>(It.IsAny<string>()))
                .ReturnsAsync("test-token");

            ctx.Services.AddScoped(_ => mockLocalStorage.Object);
        }
    }
}