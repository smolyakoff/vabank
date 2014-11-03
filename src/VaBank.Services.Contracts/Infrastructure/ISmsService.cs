namespace VaBank.Services.Contracts.Infrastructure
{
    public interface ISmsService : IService
    {
        void SendSms(SendSmsCommand command);
    }
}
