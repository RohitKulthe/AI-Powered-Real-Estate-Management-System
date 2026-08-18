using AIRealEstateManagementSystem.Models.Setting;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AIRealEstateManagementSystem.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("AI Real Estate", _emailSettings.FromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));

            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _emailSettings.Host,
                _emailSettings.Port,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _emailSettings.FromEmail,
                _emailSettings.AppPassword);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}