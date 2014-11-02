using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Contracts.Accounting
{
    public interface ICurrencyRateManagementService
    {
        void UpdateRates();

        CurrencyRatesLookupModel GetAllRates();
    }
}
