namespace VaBank.Services.Contracts.Processing.Queries
{
    public class TodayCurrencyRateQuery
    {
        public string FromISOName { get; set; }
        public string ToISOName { get; set; }
    }
}
