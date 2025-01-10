using Microsoft.AspNetCore.Mvc;
using Moq;
using NotificationService.Controllers;
using NotificationService.Models;
using NotificationService.Services;
using NotificationService.Services.Providers.Email;

namespace IsSendable;
public class NotificationControllerTests
{
    [Fact]
    public async Task SendSms_OkifPass()
    {
        // Arrange
        var mockNotificationManager = new Mock<INotificationManager>();
        mockNotificationManager
            .Setup(nm => nm.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        var controller = new NotificationController(mockNotificationManager.Object);

        var request = new SmsRequest
        {
            Channel = "Email",
            To = "test@example.com",
            Message = "Test message"
        };

        // Act
        var result = await controller.SendSms(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Sms sent successfully.", okResult.Value);
    }
    [Fact]
    public async Task SendEmail_OkifPass()
    {
        // Arrange
        var mockNotificationManager = new Mock<INotificationManager>();
        mockNotificationManager
            .Setup(nm => nm.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        var controller = new NotificationController(mockNotificationManager.Object);

        var request = new EmailRequest
        {
            Sender = "Email",
            Receiver = "test@example.com",
            Message = "Test message"
        };

        // Act
        var result = await controller.SendEmail(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Email sent successfully.", okResult.Value);
    }

   
}