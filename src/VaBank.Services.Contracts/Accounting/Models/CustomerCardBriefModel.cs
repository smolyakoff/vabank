using System;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CustomerCardBriefModel
    {
        public Guid CardId { get; set; }

        public string AccountNo { get; set; }

        public string SecureCardNo { get; set; }

        public string CardholderFirstName { get; set; }

        public string CardholderLastName { get; set; }

        public DateTime ExpirationDateUtc { get; set; }

        public CardVendorModel CardVendor { get; set; }

        public CurrencyModel Currency { get; set; }

        public decimal Balance { get; set; }

        public string FriendlyName { get; set; }

        public UserNameModel Owner { get; set; }
    }
}
