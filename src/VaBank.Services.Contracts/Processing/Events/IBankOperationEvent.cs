using VaBank.Services.Contracts.Common.Events;

namespace VaBank.Services.Contracts.Processing.Events
{
    public interface IBankOperationEvent : IAuditedEvent
    {
        long BankOperationId { get; set; }
    }
}
