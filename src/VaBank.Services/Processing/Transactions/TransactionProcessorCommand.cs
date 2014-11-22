using System;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Contracts.Common.Commands;

namespace VaBank.Services.Processing.Transactions
{
    public class TransactionProcessorCommand : IOperationCommand
    {
        public TransactionProcessorCommand(Guid operationId, Transaction transaction, BankOperation bankOperation)
        {
            Argument.NotNull(transaction, "transaction");
            OperationId = operationId;
            Transaction = transaction;
            BankOperation = bankOperation;
        }

        public Guid OperationId { get; private set; }

        public Transaction Transaction { get; private set; }

        public BankOperation BankOperation { get; private set; }
        
    }
}
