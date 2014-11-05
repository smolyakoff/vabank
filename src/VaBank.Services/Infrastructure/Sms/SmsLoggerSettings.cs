using System.Collections.Generic;
using VaBank.Common.Util;

namespace VaBank.Services.Infrastructure.Sms
{
    [Settings("VaBank.Services.Infrastructure.Sms.SmsLoggerSettings")]
    internal class SmsLoggerSettings
    {
        public SmsLoggerSettings()
        {
            EmailAddresses = new List<string>();
        }

        public List<string> EmailAddresses { get; set; } 
    }
}
