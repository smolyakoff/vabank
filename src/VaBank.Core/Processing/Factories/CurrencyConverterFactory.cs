using System;
using System.Linq;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Repositories;

namespace VaBank.Core.Processing.Factories
{
    internal class CurrencyConverterFactory
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly ExchangeRateSettings _settings;

        public CurrencyConverterFactory(IExchangeRateRepository exchangeRateRepository)
        {
            Argument.NotNull(exchangeRateRepository, "exchangeRateRepository");
            _exchangeRateRepository = exchangeRateRepository;
            _settings = new ExchangeRateSettings();
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
            var rate = rates.OrderBy(x => _settings.GetPriority(x.Base.ISOName)).First();
            return rate.Converter;
        }
    }
}
