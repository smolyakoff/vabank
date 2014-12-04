using System.Collections.Generic;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing
{
    public interface IExchangeRateService
    {
        IList<ExchangeRateModel> GetLocalCurrencyRates();

        void UpdateRates();

        IList<CurrencyModel> GetSupportedCurrencies();
    }
}
