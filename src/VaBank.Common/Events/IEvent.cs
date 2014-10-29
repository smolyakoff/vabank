using System;

namespace VaBank.Common.Events
{
    public interface IEvent
    {
        DateTime TimestampUtc { get; } 
    }
}
