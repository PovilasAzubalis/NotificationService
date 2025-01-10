namespace NotificationService.Services
{
    public interface INotificationManager
    {
        Task<bool> SendSmsAsync(string channel, string to, string message);
        Task SendEmailAsync(string recipientEmail, string subject, string message);
    }
}
