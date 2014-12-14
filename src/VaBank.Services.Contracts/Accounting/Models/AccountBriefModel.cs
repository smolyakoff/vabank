using System;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Accounting.Models
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
