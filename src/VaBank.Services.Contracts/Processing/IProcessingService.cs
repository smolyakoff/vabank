using System;

using VaBank.Common.Data;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing
{
    public interface IProcessingService
    {
        OperationProcessingResult ProcessBankOperation(ProcessBankOperationCommand command);

        TransactionProcessingResult ProcessTransaction(ProcessTransactionCommand command);

        CardTransactionModel GetCardTransaction(IdentityQuery<Guid> id);
    }
}
