using System;
using VaBank.Common.Util;

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
