using VaBank.Services.Common;
using VaBank.Services.Contracts.Infrastructure;

namespace VaBank.Services.Infrastructure
{
    public class SmsService : BaseService, ISmsService
    {
        public SmsService(BaseServiceDependencies dependencies) : base(dependencies)
        {
        }

        public void SendSms(SendSmsCommand command)
        {
            EnsureIsValid(command);
            throw new System.NotImplementedException();
        }
    }
}
