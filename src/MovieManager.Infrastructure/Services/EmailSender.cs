using Microsoft.Extensions.Logging;
using MovieManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Core.Settings;
using Microsoft.Extensions.Options;
using System.Net.Mime;

namespace MovieManager.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IAppLogger<EmailSender> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailSender(IAppLogger<EmailSender> logger, IOptionsSnapshot<EmailSettings> emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        public void SendEmail(string from, string to, string subject, string body)
        {
            var emailClient = new SmtpClient(_emailSettings.MailServer);
            emailClient.Port = _emailSettings.SmtpPort;
            emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            emailClient.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(_emailSettings.NetworkCredentialuserName, _emailSettings.NetworkCredentialpassword);
            emailClient.EnableSsl = _emailSettings.EnableSsl;
            emailClient.Credentials = credentials;

            var message = new MailMessage
            {

                From = new MailAddress(from ?? _emailSettings.FromEmail),
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(to));
            
            ContentType mimeType = new System.Net.Mime.ContentType("text/html");
            mimeType.CharSet = "UTF-8";
            AlternateView alternate = AlternateView.CreateAlternateViewFromString(message.Body, mimeType);
            message.AlternateViews.Add(alternate);

            emailClient.Send(message);
            _logger.LogWarning($"Sending email to {to} from {from} with subject {subject}.");
        }
    }
}
