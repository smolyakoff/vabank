using System;
using VaBank.Common.Events;

namespace VaBank.Services.Contracts.Common.Events
{
    public interface IApplicationEvent : IEvent
    {
        Guid? UserId { get; }
    }
}
