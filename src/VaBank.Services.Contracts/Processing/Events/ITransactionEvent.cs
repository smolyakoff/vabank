using System;
using VaBank.Services.Contracts.Common.Events;

namespace VaBank.Services.Contracts.Processing.Events
{
    public interface ITransactionEvent : IAuditedEvent
    {
        Guid TransactionId { get; }

        long? BankOperationId { get; }
    }
}
