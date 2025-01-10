using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace NotificationService.Services.Providers.Email
{
    public class EmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;

        public EmailService(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {
            var from = new EmailAddress("your_verified_sender_email@example.com", "Your Name");
            var to = new EmailAddress(recipientEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await _sendGridClient.SendEmailAsync(msg);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to send email. Status code: {response.StatusCode}");
            }
        }
    }
}