using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using Pronia.Abstractions;
using Pronia.ViewModels.EmailViewModel;
using MailKit.Net.Smtp;

namespace Pronia.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettingsVm _settings;
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

            _settings = _configuration.GetSection("SmtpSettings").Get<SmtpSettingsVm>() ?? new();
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));

                message.To.Add(new MailboxAddress(email, email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };

                using SmtpClient client = new();

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(_settings.Server, _settings.Port);

                await client.AuthenticateAsync(_settings.Username, _settings.Password);

                await client.SendAsync(message);

            }
            catch (Exception exp)
            {

                throw;
            }


        }



    }
}
