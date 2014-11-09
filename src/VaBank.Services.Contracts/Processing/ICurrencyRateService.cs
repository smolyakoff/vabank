using System.Collections.Generic;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing
{
    public interface ICurrencyRateService
    {
        IList<ExchangeRateModel> GetLocalCurrencyRates();

        void UpdateRates();
    }
}
