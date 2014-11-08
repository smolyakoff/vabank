using System;
using VaBank.Common.Events;

namespace VaBank.Services.Contracts.Common.Events
{
    public interface IOperationEvent : IEvent
    {
        Guid OperationId { get; }
    }
}
