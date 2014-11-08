using System;

namespace VaBank.Services.Contracts.Processing.Queries
{
    public class CurrencyRateQuery : TodayCurrencyRateQuery
    {
        public DateTime TimestampUtc { get; set; }
    }
}
