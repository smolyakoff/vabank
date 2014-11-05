using VaBank.Common.Util;

namespace VaBank.Services.Infrastructure.Sms
{
    [Settings("VaBank.Services.Infrastructure.Sms.TwilioClientSettings")]
    internal class TwilioClientSettings
    {
        public string AccountSid { get; set; }

        public string AuthToken { get; set; }
    }
}
