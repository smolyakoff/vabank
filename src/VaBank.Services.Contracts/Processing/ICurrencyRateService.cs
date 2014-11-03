using System;
using VaBank.Services.Contracts.Processing.Models;
using VaBank.Services.Contracts.Processing.Queries;

namespace VaBank.Services.Contracts.Processing
{
    public interface ICurrencyRateService
    {
        void UpdateRates();
        CurrencyRatesLookupModel GetAllTodayRates();
        CurrencyRatesLookupModel GetAllRates(DateTime date);
        CurrencyRateModel GetTodayRate(TodayCurrencyRateQuery query);
        CurrencyRateModel GetRate(CurrencyRateQuery query);
    }
}
