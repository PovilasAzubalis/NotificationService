using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace NotificationService.Services.Providers
{
    public class TwilioProvider : INotificationProvider
    {
        private readonly string? _accountSid;
        private readonly string? _authToken;
        private readonly string? _fromNumber;

        public TwilioProvider(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _fromNumber = configuration["Twilio:FromNumber"];
            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task<bool> SendAsync(string to, string message)
        {
            try
            {
                var messageResponse = await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(_fromNumber),
                    to: new Twilio.Types.PhoneNumber(to)
                );
                return messageResponse.ErrorCode == null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Twilio error: {ex.Message}");
                return false;
            }
        }
    }
}
