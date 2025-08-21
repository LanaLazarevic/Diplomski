using Microsoft.Extensions.Options;
using PFM.Application.Interfaces;
using PFM.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Infrastructure.Services
{
    public class EmailService : IEmailService, IDisposable
    {
        private readonly SmtpClient _client;
        private readonly string _from;

        public EmailService(IOptions<EmailSettings> options)
        {
            var settings = options.Value;
            _from = settings.From ?? settings.Username ?? string.Empty;

            _client = new SmtpClient(settings.Host, settings.Port)
            {
                EnableSsl = true
            };

            if (!string.IsNullOrEmpty(settings.Username))
            {
                _client.Credentials = new NetworkCredential(settings.Username, settings.Password);
            }
        }

        public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, Stream attachment, string fileName, CancellationToken ct = default)
        {
            using var message = new MailMessage(_from, to, subject, body);
            attachment.Position = 0;
            message.Attachments.Add(new Attachment(attachment, fileName));
            await _client.SendMailAsync(message, ct);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
