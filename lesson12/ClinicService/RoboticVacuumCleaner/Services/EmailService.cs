using RoboticVacuumCleaner.Server.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace RoboticVacuumCleaner.Server.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailConfirmationAsync(string email, string token)
        {
            var confirmationLink = $"{_configuration["AppUrl"]}/confirm-email?email={email}&token={token}";

            var subject = "Подтверждение email для Robot Cleaner";
            var body = $@"
                <h1>Добро пожаловать в Robot Cleaner!</h1>
                <p>Пожалуйста, подтвердите ваш email, перейдя по ссылке:</p>
                <a href='{confirmationLink}'>Подтвердить email</a>
                <p>Если вы не регистрировались, проигнорируйте это письмо.</p>
            ";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string email, string token)
        {
            var resetLink = $"{_configuration["AppUrl"]}/reset-password?email={email}&token={token}";

            var subject = "Сброс пароля Robot Cleaner";
            var body = $@"
                <h1>Сброс пароля</h1>
                <p>Для сброса пароля перейдите по ссылке:</p>
                <a href='{resetLink}'>Сбросить пароль</a>
                <p>Если вы не запрашивали сброс пароля, проигнорируйте это письмо.</p>
            ";

            await SendEmailAsync(email, subject, body);
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("EmailSettings");
                var from = smtpSettings["FromEmail"];
                var smtpServer = smtpSettings["SmtpServer"];
                var port = int.Parse(smtpSettings["Port"] ?? "587");
                var username = smtpSettings["Username"];
                var password = smtpSettings["Password"];

                using var client = new SmtpClient(smtpServer, port);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(username, password);

                var message = new MailMessage(from, to, subject, body);
                message.IsBodyHtml = true;

                await client.SendMailAsync(message);

                _logger.LogInformation($"Email sent to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {to}");
                throw;
            }
        }
    }
}