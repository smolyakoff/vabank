using System;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CardModel
    {
        public Guid CardId { get; set; }

        public string CardNo { get; set; }

        public CardVendorModel CardVendor { get; set; }

        public string CardholderFirstName { get; set; }

        public string CardholderLastName { get; set; }

        public DateTime ExpirationDateUtc { get; set; }
    }
}
