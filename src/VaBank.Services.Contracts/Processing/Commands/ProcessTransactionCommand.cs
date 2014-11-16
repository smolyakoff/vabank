using System;

namespace VaBank.Services.Contracts.Processing.Commands
{
    public class ProcessTransactionCommand
    {
        public Guid TransactionId { get; set; }

        public long? OperationId { get; set; }
    }
}
