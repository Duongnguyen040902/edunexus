using MailKit.Net.Smtp;
using MimeKit;
using sep.backend.v1.Services.IServices;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace sep.backend.v1.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string body = null)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("sep401", _configuration["MailCatcher:From"]));
            emailMessage.To.Add(new MailboxAddress("test", email));
            emailMessage.Subject = subject;

            if (string.IsNullOrEmpty(body))
            {
                // body = Helpers.TemplateHelper.GetEmailTemplate("default.html");
                body = "body";
            }

            emailMessage.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_configuration["MailCatcher:Host"], int.Parse(_configuration["MailCatcher:SmtpPort"]),
                _configuration.GetValue<bool>("MailCatcher:EnableSsl"));

            var username = _configuration["MailCatcher:Username"];
            var password = _configuration["MailCatcher:Password"];
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                await client.AuthenticateAsync(username, password);
            }

            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}