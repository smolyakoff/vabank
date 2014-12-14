using System;
using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class AccountBriefModel
    {
        public string AccountNo { get; set; }

        public CurrencyModel Currency { get; set; }

        public decimal Balance { get; set; }

        public IOwnerModel Owner { get; set; }

        public DateTime OpenDateUtc { get; set; }

        public DateTime ExpirationDateUtc { get; set; }
    }
}
