using System.Collections.Generic;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing.Repositories
{
    public interface IExchangeRateRepository : IRepository
    {
        void Save(ExchangeRate exchangeRate);

        IList<ExchangeRate> GetActualForExchange(ExchangeRateKey key);

        IList<ExchangeRate> GetAllActual(string baseCurrencyISOName);
    }
}
