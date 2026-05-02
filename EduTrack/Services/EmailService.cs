using System;
using System.IO;
using System.Threading.Tasks;
using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace EduTrack.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            var body = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = body.ToMessageBody();

            if (string.Equals(_settings.DeliveryMethod, "PickupDirectory", StringComparison.OrdinalIgnoreCase))
            {
                // Ensure directory exists
                var dir = Path.IsPathRooted(_settings.PickupDirectory) ? _settings.PickupDirectory : Path.Combine(Directory.GetCurrentDirectory(), _settings.PickupDirectory);
                Directory.CreateDirectory(dir);

                // Save as .eml file
                var fileName = Path.Combine(dir, $"email_{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid()}.eml");
                await using var stream = File.Create(fileName);
                await message.WriteToAsync(stream);
                return;
            }

            // Otherwise send using SMTP
            using var client = new SmtpClient();
            if (_settings.UseSsl)
            {
                await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect);
            }
            else
            {
                await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
            }

            if (!string.IsNullOrWhiteSpace(_settings.Username))
            {
                await client.AuthenticateAsync(_settings.Username, _settings.Password ?? string.Empty);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
