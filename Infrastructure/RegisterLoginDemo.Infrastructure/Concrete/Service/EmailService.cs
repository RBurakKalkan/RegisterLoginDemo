using Hangfire;
using Microsoft.Extensions.Configuration;
using RegisterLoginDemo.Application.Abstraction.Service;
using System.Net.Mail;

namespace RegisterLoginDemo.Infrastructure.Concrete.Service
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailService(IConfiguration _config)
        {
            _smtpServer = _config.GetValue<string>("SmsServiceProvider:SmtpServer");
            _smtpPort = _config.GetValue<int>("SmsServiceProvider:SmtpPort");
            _smtpUsername = _config.GetValue<string>("SmsServiceProvider:SmtpUsername"); ;
            _smtpPassword = _config.GetValue<string>("SmsServiceProvider:SmtpPassword"); ;
        }
        public void SendVerificationCode(string recipient, string message, string subject)
        {
            BackgroundJob.Enqueue(() => SendEmail(recipient, message, subject));
        }
        public void SendEmail(string recipient, string message, string subject)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential(_smtpUsername, _smtpPassword);
                    smtpClient.EnableSsl = true;

                    // Create the email message
                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(_smtpUsername);
                    mailMessage.To.Add(new MailAddress(recipient));
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;

                    // Send the email
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
