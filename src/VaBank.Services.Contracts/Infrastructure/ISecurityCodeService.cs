namespace VaBank.Services.Contracts.Infrastructure
{
    public interface ISecurityCodeService
    {
        SecurityCodeTicketModel GenerateSecurityCode();
    }
}
