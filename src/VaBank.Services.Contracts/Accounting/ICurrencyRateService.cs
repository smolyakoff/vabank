using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Contracts.Accounting
{
    public interface ICurrencyRateService
    {
        void UpdateRates();

        CurrencyRatesLookupModel GetAllRates();
    }
}
