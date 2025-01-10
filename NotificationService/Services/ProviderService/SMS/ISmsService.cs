namespace NotificationService.Services.Providers.SMS
{

    public interface ISmsService
    {
        Task<bool> SendAsync(string to, string message);
    }
}

