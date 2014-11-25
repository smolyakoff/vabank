using System;
using VaBank.Core.App.Entities;
using VaBank.Core.Common.History;

namespace VaBank.Core.Processing.Entities
{
    [Historical(typeof(HistoricalTransactionSpecification))]
    public class HistoricalTransaction : ITransaction, IHistoricalEntity<HistoricalTransaction>
    {
        internal HistoricalTransaction()
        { 
        }
        
        public long HistoryId { get; internal set; }

        public DateTime HistoryTimestampUtc { get; internal set; }

        public Guid HistoryOperationId { get; internal set; }

        public string HistoryAction { get; internal set; }

        public Guid Id { get; internal set; }

        public string AccountNo { get; internal set; }

        public string CurrencyISOName { get; internal set; }

        public decimal TransactionAmount { get; internal set; }

        public decimal AccountAmount { get; internal set; }

        public decimal RemainingBalance { get; internal set; }

        public DateTime CreatedDateUtc { get; internal set; }

        public DateTime? PostDateUtc { get; internal set; }

        public string Code { get; internal set; }

        public string Description { get; internal set; }

        public string Location { get; internal set; }

        public string ErrorMessage { get; internal set; }

        public ProcessStatus Status { get; internal set; }

        public virtual Operation HistoryOperation { get; internal set; }
    }
}
