namespace NotificationService.Models
{

    public class EmailRequest
    {
        public string? RecipientEmail { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
    }
}
