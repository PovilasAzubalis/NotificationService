namespace NotificationService.Models
{

    public class EmailRequest
    {
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public string? Message { get; set; }
    }
}
