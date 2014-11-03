namespace VaBank.Services.Contracts.Processing.Queries
{
    public class TodayCurrencyRateQuery
    {
        public string BuyingCurrencyISOName { get; set; }
        public string SellingCurrencyISOName { get; set; }
    }
}
