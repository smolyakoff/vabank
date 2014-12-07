using VaBank.Services.Contracts.Processing.Models;
using VaBank.Services.Contracts.Transfers.Commands;
using VaBank.Services.Contracts.Transfers.Models;

namespace VaBank.Services.Contracts.Transfers
{
    public interface ICardTransferClientService
    {
        BankOperationModel Transfer(PersonalCardTransferCommand command);

        BankOperationModel Transfer(InterbankCardTransferCommand command);

        CardTransferLookupModel GetLookup();
    }
}
