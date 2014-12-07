using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Infrastructure.Sms;
using VaBank.Services.Contracts.Membership;
using VaBank.Services.Contracts.Processing;

namespace VaBank.Jobs.Infrastructure
{
    public class SmsNotificationJobContext : DefaultJobContext<ISmsEvent>
    {
        public ISmsService SmsService { get; set; }

        public IUserService UserService { get; set; }

        public IProcessingService ProcessingService { get; set; }

        public ICardAccountService CardAccountService { get; set; }
    }
}
