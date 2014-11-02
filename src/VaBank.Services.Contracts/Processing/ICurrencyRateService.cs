using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing
{
    public interface ICurrencyRateService
    {
        void UpdateRates();

        CurrencyRatesLookupModel GetAllRates();
    }
}
