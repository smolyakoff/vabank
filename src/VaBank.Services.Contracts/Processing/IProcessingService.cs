using System;

using VaBank.Common.Data;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing
{
    public interface IProcessingService
    {
        BankOperationModel ProcessBankOperation(ProcessBankOperationCommand command);

        TransactionModel ProcessTransaction(ProcessTransactionCommand command);

        CardTransactionModel GetCardTransaction(IdentityQuery<Guid> id);
    }
}
