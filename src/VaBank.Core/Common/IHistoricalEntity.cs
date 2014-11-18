using System;
using System.Linq.Expressions;

namespace VaBank.Core.Common
{
    public interface IHistoricalEntity<T>
         where T : class, IHistoricalEntity<T>
    {
        long HistoryId { get; }
        char HistoryAction { get; }
        DateTime HistoryTimestampUtc { get; }
        Guid HistoryOperationId { get; }
        string SurrogateKeyPropertyName { get; }
    }
}
