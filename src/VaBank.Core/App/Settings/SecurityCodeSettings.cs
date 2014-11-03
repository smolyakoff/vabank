using System;
using VaBank.Core.Common;

namespace VaBank.Core.App.Settings
{
    [Settings("VaBank.Core.App.Settings.SecurityCodeSettings")]
    public class SecurityCodeSettings
    {
        public SecurityCodeSettings()
        {
            SmsCodeExpirationPeriod = TimeSpan.FromMinutes(5);
        }

        public TimeSpan SmsCodeExpirationPeriod { get; set; }
    }
}
