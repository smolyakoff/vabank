using System;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class ExchangeRateModel
    {
        public string BaseCurrencyISOName { get; set; }

        public string ForeignCurrencyISOName { get; set; }

        public decimal BuyRate { get; set; }

        public decimal SellRate { get; set; }

        public DateTime TimestampUtc { get; set; }
    }
}
