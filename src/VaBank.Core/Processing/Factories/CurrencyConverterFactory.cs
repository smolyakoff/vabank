using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Repositories;

namespace VaBank.Core.Processing.Factories
{
    [Injectable]
    public class CurrencyConverterFactory
    {
        private static readonly SortedList<string, int> CurrencyPriorityList = new SortedList<string, int>
        {
            {"BYR", 0},
            {"USD", 1}
        };

        private readonly IExchangeRateRepository _exchangeRateRepository;

        public CurrencyConverterFactory(IExchangeRateRepository exchangeRateRepository)
        {
            Argument.NotNull(exchangeRateRepository, "exchangeRateRepository");
            _exchangeRateRepository = exchangeRateRepository;
        }

        public CurrencyConverter Create(CurrencyConversion conversion)
        {
            var rates = _exchangeRateRepository.GetActualForExchange(conversion.ExchangeRateKey);
            if (rates.Count == 0)
            {
                var message = string.Format("Conversion {0} is not supported.", conversion);
                throw new NotSupportedException(message);
            }
            if (rates.Count == 1)
            {
                return rates[0].Converter;
            }
            var rate = rates.OrderBy(GetPriority).First();
            return rate.Converter;
        }

        private static int GetPriority(ExchangeRate rate)
        {
            return !CurrencyPriorityList.ContainsKey(rate.Base.ISOName) 
                ? int.MaxValue 
                : CurrencyPriorityList[rate.Base.ISOName];
        }
    }
}
