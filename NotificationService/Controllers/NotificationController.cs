using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationManager _notificationManager;

        public NotificationController(NotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            var result = await _notificationManager.SendNotificationAsync(request.Channel, request.To, request.Message);
            return result ? Ok("Notification sent successfully.") : StatusCode(500, "Notification failed, retry scheduled.");
        }
    }
}
