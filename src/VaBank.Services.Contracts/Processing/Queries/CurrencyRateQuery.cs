using System;

namespace VaBank.Services.Contracts.Processing.Queries
{
    public class CurrencyRateQuery : TodayCurrencyRateQuery
    {
        public DateTime Date { get; set; }
    }
}
