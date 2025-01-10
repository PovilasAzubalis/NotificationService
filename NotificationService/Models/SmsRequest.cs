namespace NotificationService.Models
{
    
    public class SmsRequest
    {
        public string? Channel { get; set; }
        public string? To { get; set; }
        public string? Message { get; set; }
    }
}
