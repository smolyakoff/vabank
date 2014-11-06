using System;

namespace VaBank.Core.Common
{
    public abstract class HistoricalEntity : Entity
    {
        public DateTime HistoryTimestampUtc { get; protected set; }

        public Guid HistoryOperationId { get; protected set; }

        public long HistoryId { get; protected set; }
    }
}
