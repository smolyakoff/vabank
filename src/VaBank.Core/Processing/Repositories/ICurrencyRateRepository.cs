using System;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing.Repositories
{
    public interface ICurrencyRateRepository
    {
        CurrencyRate GetRate(CurrencyNamePair currencies, DateTime date);
        CurrencyRate GetTodayRate(CurrencyNamePair currencies);
    }
}
