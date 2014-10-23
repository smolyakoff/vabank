using System.Collections.Generic;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class AccountingLookupModel
    {
        public AccountingLookupModel()
        {
            Currencies = new List<CurrencyModel>();
        }

        public List<CurrencyModel> Currencies { get; set; }
    }
}
