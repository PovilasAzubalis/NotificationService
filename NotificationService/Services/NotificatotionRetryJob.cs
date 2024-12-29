using Quartz;

namespace NotificationService.Services;

public class NotificationRetryJob : IJob
{
    private readonly NotificationManager _notificationManager;

    public NotificationRetryJob(NotificationManager notificationManager)
    {
        _notificationManager = notificationManager;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var jobData = context.MergedJobDataMap;
        var channel = jobData.GetString("channel");
        var to = jobData.GetString("to");
        var message = jobData.GetString("message");

        var success = await _notificationManager.SendNotificationAsync(channel, to, message);
        if (!success)
        {
            Console.WriteLine("Retry attempt failed. Will try again later.");
        }
        else
        {
            Console.WriteLine("Notification successfully sent on retry.");
        }
    }
}