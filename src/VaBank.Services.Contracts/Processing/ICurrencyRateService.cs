using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing
{
    public interface ICurrencyRateService
    {
        ExchangeRatesTableModel GetLocalCurrencyRates();

        void UpdateRates();
    }
}
