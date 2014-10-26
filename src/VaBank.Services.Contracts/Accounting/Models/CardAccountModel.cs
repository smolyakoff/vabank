using System;

namespace VaBank.Services.Contracts.Accounting.Models
{
    //TODO: what is it for?
    public class CardAccountModel : CardAccountBriefModel
    {
        public CardVendorModel CardVendor { get; set; }

        public string CardNo { get; set; }

        public string CardholderFirstName { get; set; }

        public string CardholderLastName { get; set; }

        public DateTime CardExpirationDateUtc { get; set; }

        public DateTime AccountOpenedDateUtc { get; set; }

        public DateTime AccountExpirationDateUtc
        {
            get { return CardExpirationDateUtc; }
        }

        public CardLimitsModel CardLimits { get; set; }
    }
}
