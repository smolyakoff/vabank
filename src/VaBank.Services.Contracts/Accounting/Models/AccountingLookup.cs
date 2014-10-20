using System.Collections.Generic;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class AccountingLookup
    {
        public AccountingLookup()
        {
            Currencies = new List<CurrencyModel>();
        }

        public List<CurrencyModel> Currencies { get; set; }
    }
}
