using System;

namespace VaBank.Core.Common.History
{
    public interface IHistoricalEntity<T>
         where T : class, IHistoricalEntity<T>
    {
        long HistoryId { get; }
        DateTime HistoryTimestampUtc { get; }
        Guid HistoryOperationId { get; }
        string HistoryAction { get; }
    }
}
