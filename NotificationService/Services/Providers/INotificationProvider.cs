namespace NotificationService.Services.Providers
{
    public interface INotificationProvider
    {
        Task<bool> SendAsync(string to, string message);
    }
}
