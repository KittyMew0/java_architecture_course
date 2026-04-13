namespace RoboticVacuumCleaner.Server.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailConfirmationAsync(string email, string token);
        Task SendPasswordResetEmailAsync(string email, string token);
    }
}
