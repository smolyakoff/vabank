using System;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class UserCardModel
    {
        public Guid UserId { get; set; }

        public Guid CardId { get; set; }

        public CardVendorModel CardVendor { get; set; }

        public string SecureCardNo { get; set; }

        public DateTime ExpirationDateUtc { get; set; }

        public CurrencyModel Currency { get; set; }

        public decimal Balance { get; set; }

        public string FriendlyName { get; set; }

        public bool Blocked { get; set; }
    }
}
