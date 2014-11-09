using System.Collections.Generic;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class CurrencyRatesLookupModel
    {
        public List<ExchangeRateModel> CurrencyRates { get; set; }
    }
}
