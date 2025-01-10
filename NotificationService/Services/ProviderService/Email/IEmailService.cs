namespace NotificationService.Services.Providers.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string RecipientEmail, string subject, string message);
    }
}
