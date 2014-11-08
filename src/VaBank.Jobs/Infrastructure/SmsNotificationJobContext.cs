using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Infrastructure.Sms;
using VaBank.Services.Contracts.Membership;

namespace VaBank.Jobs.Infrastructure
{
    public class SmsNotificationJobContext : DefaultJobContext<ISmsEvent>
    {
        public ISmsService SmsService { get; set; }

        public IUserService UserService { get; set; }
    }
}
