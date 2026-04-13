using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RoboticVacuumCleaner.Web;
using RoboticVacuumCleaner.Web.Services;
using RoboticVacuumCleaner.Web.Services.Interfaces;

namespace RoboticVacuumCleaner.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Register HTTP Client
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            // Register services
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRobotService, RobotService>();

            // Add authorization
            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}
