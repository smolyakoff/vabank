using System;
using System.Collections.Generic;
using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class TransactionLogEntryModel
    {
        public TransactionLogEntryModel()
        {
            Versions = new List<TransactionLogEntryHistoricalModel>();
        }

        public Guid TransactionId { get; set; }

        public CardAccountBriefModel Account { get; set; }

        public List<TransactionLogEntryHistoricalModel> Versions { get; set; } 
    }
}
