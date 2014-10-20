using System;
using VaBank.Common.Events;

namespace VaBank.Services.Contracts.Common
{
    public interface IAuditedEvent : IEvent
    {
        Guid OperationId { get; }

        string Code { get; }

        string Description { get; }

        object Data { get; }
    }
}
