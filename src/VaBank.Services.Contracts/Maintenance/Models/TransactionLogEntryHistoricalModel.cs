using System;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class TransactionLogEntryHistoricalModel : TransactionLogEntryBriefModel
    {
        public DateTime TimestampUtc { get; set; }

        public Guid? ChangeOwnerUserId { get; set; }
    }
}
