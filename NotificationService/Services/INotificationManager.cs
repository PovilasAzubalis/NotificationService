namespace NotificationService.Services
{
    public interface INotificationManager
    {
        Task<bool> SendSmsAsync(string channel, string to, string message);
        Task SendEmailAsync(string Sender, string Receicer, string message);
    }
}
