namespace VaBank.Services.Contracts.Infrastructure.Secuirty
{
    public interface ISecurityCodeService
    {
        SecurityCodeTicketModel GenerateSecurityCode();
    }
}
