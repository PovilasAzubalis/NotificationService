using Quartz;

namespace NotificationService.Services.Providers.Email
{
    public class EmailJob : IJob
    {
        private readonly IEmailService _emailService;

        public EmailJob(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            string recipientEmail = "recipient@example.com";
            string subject = "Scheduled Notification";
            string message = "This is a scheduled email notification.";

            await _emailService.SendEmailAsync(recipientEmail, subject, message);
        }
    }
}
