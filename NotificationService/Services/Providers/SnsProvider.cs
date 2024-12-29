using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace NotificationService.Services.Providers
{
    public class SnsProvider : INotificationProvider
    {
        private readonly AmazonSimpleNotificationServiceClient _snsClient;

        public SnsProvider(IConfiguration configuration)
        {
            var accessKey = configuration["SNS:AccessKey"];
            var secretKey = configuration["SNS:SecretKey"];
            var region = configuration["SNS:Region"];

            _snsClient = new AmazonSimpleNotificationServiceClient(accessKey, secretKey, RegionEndpoint.GetBySystemName(region));
        }

        public async Task<bool> SendAsync(string to, string message)
        {
            try
            {
                var request = new PublishRequest
                {
                    Message = message,
                    PhoneNumber = to
                };
                var response = await _snsClient.PublishAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SNS error: {ex.Message}");
                return false;
            }
        }
    }
}
