namespace VaBank.Services.Contracts.Infrastructure.Email
{
    public interface IEmailService
    {
        void SendEmail(SendEmailCommand command);
    }
}
