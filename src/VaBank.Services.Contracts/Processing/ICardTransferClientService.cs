using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing
{
    public interface ICardTransferClientService
    {
        BankOperationModel Transfer(PersonalCardTransferCommand command);

        BankOperationModel Transfer(InterbankCardTransferCommand command);

        CardTransferLookupModel GetLookup();
    }
}
