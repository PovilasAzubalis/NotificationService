using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationManager _notificationManager;

        public NotificationController(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }



        [HttpPost]
        [Route("send-sms")]
        public async Task<IActionResult> SendSms([FromBody] SmsRequest request)
        {
            var result = await _notificationManager.SendSmsAsync(request.Channel, request.To, request.Message);
            return result ? Ok("Sms sent successfully.") : StatusCode(500, "Notification failed, retry scheduled.");
        }

        [HttpPost]
        [Route("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                await _notificationManager.SendEmailAsync(request.Sender, request.Message, request.Receiver);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Email sending failed: {ex.Message}");
            }
        }
    }
}
