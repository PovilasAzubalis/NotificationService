namespace NotificationService.Services.Providers.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string Sender, string subject, string message);
    }
}
