namespace VaBank.Services.Contracts.Infrastructure.Sms
{
    public interface ISmsService : IService
    {
        void SendSms(SendSmsCommand command);
    }
}
