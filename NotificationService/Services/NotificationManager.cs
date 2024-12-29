using NotificationService.Services.Providers;
using Quartz;

namespace NotificationService.Services
{
    public class NotificationManager
    {
        private readonly IEnumerable<INotificationProvider> _providers;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IConfiguration _configuration;

        public NotificationManager(IEnumerable<INotificationProvider> providers, ISchedulerFactory schedulerFactory, IConfiguration configuration)
        {
            _providers = providers;
            _schedulerFactory = schedulerFactory;
            _configuration = configuration;
        }

        public async Task<bool> SendNotificationAsync(string channel, string to, string message)
        {
            foreach (var provider in _providers)
            {
                var success = await provider.SendAsync(to, message);
                if (success)
                {
                    Console.WriteLine($"Notification sent via {provider.GetType().Name}");
                    return true;
                }
            }

            Console.WriteLine("All providers failed. Scheduling retry...");
            await ScheduleRetry(channel, to, message);
            return false;
        }

        private async Task ScheduleRetry(string channel, string to, string message)
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var retryPolicy = _configuration.GetSection("RetryPolicy");
            int maxAttempts = retryPolicy.GetValue<int>("MaxAttempts");
            int delaySeconds = retryPolicy.GetValue<int>("DelaySeconds");

            var job = JobBuilder.Create<NotificationRetryJob>()
                .UsingJobData("channel", channel)
                .UsingJobData("to", to)
                .UsingJobData("message", message)
                .Build();

            var trigger = TriggerBuilder.Create()
                .StartAt(DateTimeOffset.UtcNow.AddSeconds(delaySeconds))
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(delaySeconds).WithRepeatCount(maxAttempts - 1))
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
