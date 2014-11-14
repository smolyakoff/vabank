using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing
{
    public interface IInternalTransferService
    {
        TransferModel Transfer(InternalTransferCommand command);

        TransferModel PersonalTransfer(PersonalInternalTransferCommand command);
    }
}
