using System;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CustomerCardModel
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

        public bool Blocked { get; set; }

        public CardLimitsModel CardLimits { get; set; }
    }
}
