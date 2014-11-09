using VaBank.Common.Data.Repositories;
using VaBank.Core.App.Entities;
using VaBank.Core.App.Factories;
using VaBank.Services.Common;

namespace VaBank.Services.Infrastructure.Sms
{
    public class SmsServiceDependencies : IDependencyCollection
    {
        public SecurityCodeFactory SecurityCodeFactory { get; set; }

        public IRepository<SecurityCode> SecurityCodes { get; set; } 

        public TwilioClientFactory TwilioClientFactory { get; set; }

        public SmsLogger SmsLogger { get; set; }
    }
}
