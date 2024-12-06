using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Services
{
    public class EmailConfirmCode : IEmailConfirmCode
    {
        private readonly IEmailService _emailService;

        public EmailConfirmCode(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task SendEmailAsync(string email, string subject, string body = null)
        {
            return _emailService.SendEmailAsync(email, subject);
        }
    }
}