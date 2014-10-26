using System;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CardAccountBriefModel
    {
        public string AccountNo { get; set; }

        public CurrencyModel Currency { get; set; }

        public decimal Balance { get; set; }

        public UserNameModel Owner { get; set; }

        public DateTime OpenDateUtc { get; set; }

        public DateTime ExpirationDateUtc { get; set; }
    }
}
