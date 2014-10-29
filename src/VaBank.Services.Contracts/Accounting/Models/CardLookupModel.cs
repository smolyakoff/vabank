using System.Collections.Generic;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CardLookupModel
    {
        public CardLookupModel()
        {
            Currencies = new List<CurrencyModel>();
            CardVendors = new List<CardVendorModel>();
        }

        public List<CurrencyModel> Currencies { get; set; }

        public List<CardVendorModel> CardVendors { get; set; } 
    }
}
