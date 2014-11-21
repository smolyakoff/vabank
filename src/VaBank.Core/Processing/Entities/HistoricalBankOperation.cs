using System;
using VaBank.Core.Common.History;

namespace VaBank.Core.Processing.Entities
{
    public class HistoricalBankOperation : IHistoricalEntity<HistoricalBankOperation>, IBankOperation
    {
        internal HistoricalBankOperation() { }

        public OperationCategory Category { get; internal set; }

        public ProcessStatus Status { get; internal set; }

        public DateTime CreatedDateUtc { get; internal set; }

        public DateTime? CompletedDateUtc { get; internal set; }

        public string ErrorMessage { get; internal set; }

        public long Id { get; internal set; }

        public long HistoryId { get; internal set; }

        public DateTime HistoryTimestampUtc { get; internal set; }

        public Guid HistoryOperationId { get; internal set; }

        public char HistoryAction { get; internal set; }
    }
}
