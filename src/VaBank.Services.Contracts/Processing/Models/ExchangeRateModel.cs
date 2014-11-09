using System;
using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class ExchangeRateModel
    {
        public CurrencyModel BaseCurrency { get; set; }

        public CurrencyModel ForeignCurrency { get; set; }

        public decimal BuyRate { get; set; }

        public decimal SellRate { get; set; }

        public DateTime TimestampUtc { get; set; }
    }
}
