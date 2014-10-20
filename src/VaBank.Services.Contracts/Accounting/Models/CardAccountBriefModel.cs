using System;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CardAccountBriefModel
    {
        public Guid CardId { get; set; }

        public string AccountNo { get; set; }

        public CurrencyModel Currency { get; set; }

        public decimal Balance { get; set; }

        public UserBriefModel User { get; set; }

        public string SecureCardNo { get; set; }

        public bool CardBlocked { get; set; }
    }
}
