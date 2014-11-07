using System;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class CurrencyRateModel
    {
        public string FromISOName { get; set; }

        public string ToCurrencyISOName { get; set; }

        public decimal BuyingRate { get; set; }

        public decimal SellingRate { get; set; }

        public DateTime TimestampUtc { get; set; }
    }
}
