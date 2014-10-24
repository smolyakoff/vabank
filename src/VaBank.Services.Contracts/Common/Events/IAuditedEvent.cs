using System;
using VaBank.Common.Events;

namespace VaBank.Services.Contracts.Common.Events
{
    public interface IAuditedEvent : IEvent
    {
        Guid OperationId { get; }

        string Code { get; }

        string Description { get; }

        object Data { get; }
    }
}
