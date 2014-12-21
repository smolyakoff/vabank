using System;

namespace VaBank.Services.Contracts.Statistics.Models
{
    public class ProcessedTransactionStatsModel
    {
        public DateTime Date { get; set; }

        public long TransactionsCount { get; set; }
    }
}
