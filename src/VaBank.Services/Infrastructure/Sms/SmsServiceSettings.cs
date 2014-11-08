using VaBank.Common.Util;

namespace VaBank.Services.Infrastructure.Sms
{
    [Settings("VaBank.Services.Infrastructure.Sms.SmsServiceSettings")]
    internal class SmsServiceSettings
    {
        public bool UseLogger { get; set; }

        public string OutboundPhoneNumber { get; set; }
    }
}
