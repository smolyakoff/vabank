using System;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Contracts.Common.Commands;

namespace VaBank.Services.Processing.Operations
{
    public class BankOperationProcessorCommand : IOperationCommand
    {
        internal BankOperationProcessorCommand(Guid operationId, BankOperation bankOperation)
        {
            Argument.NotNull(bankOperation, "bankOperation");
            OperationId = operationId;
            BankOperation = bankOperation;
        }

        public Guid OperationId { get; private set; }

        public BankOperation BankOperation { get; private set; }
    }
}
