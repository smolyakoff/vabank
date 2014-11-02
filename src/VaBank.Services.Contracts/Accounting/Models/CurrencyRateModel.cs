using System;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CurrencyRateModel
    {
        public string BuyingCurrencyISOName { get; set; }

        public string SellingCurrencyISOName { get; set; }

        public decimal Rate { get; set; }

        public DateTime Date { get; set; }
    }
}
