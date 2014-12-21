using System;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Util;
using VaBank.Common.Util.Math;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing
{    
    [Injectable]
    public class ExchangeRateCalculator
    {
        private readonly IRepository<Currency> _currencyRepository;

        private readonly ExchangeRateSettings _settings;
        private readonly NationalExchangeRateRoundingSettings _roundingSettings;

        public ExchangeRateCalculator(IRepository<Currency> currencyRepository, 
            NationalExchangeRateRoundingSettings roundingSettings)
        {
            Argument.NotNull(currencyRepository, "currencyRepository");
            Argument.NotNull(roundingSettings, "roundingSettings");

            _roundingSettings = roundingSettings;
            _currencyRepository = currencyRepository;
            _settings = new ExchangeRateSettings();
        }

        public ExchangeRate CalculateFromNationalBankRate(string foreignCurrencyISOName, decimal rate, DateTime timestampUtc)
        {
            Argument.NotEmpty(foreignCurrencyISOName, "foreignCurrencyISOName");
            Argument.Satisfies(foreignCurrencyISOName, x => x != _settings.NationalCurrency);
            Argument.Satisfies(rate, x => x > 0, "rate");

            var conversionRate = new ConversionRate(new CurrencyConversion(_settings.NationalCurrency, foreignCurrencyISOName), rate);
            return CalculateFromConversionRate(conversionRate, timestampUtc, _roundingSettings.GetRounding(foreignCurrencyISOName));
        }

        public ExchangeRate CalculateFromConversionRate(ConversionRate conversionRate, DateTime timstampUtc, Rounding rounding)
        {
            Argument.NotNull(conversionRate, "conversionRate");

            var baseCurrency = _currencyRepository.Find(conversionRate.Conversion.From);
            if (baseCurrency == null)
            {
                var message = string.Format("Can't find currency [{0}] in the database.", conversionRate.Conversion.From);
                throw new InvalidOperationException(message);
            }
            var foreignCurrency = _currencyRepository.Find(conversionRate.Conversion.To);
            if (foreignCurrency == null)
            {
                var message = string.Format("Can't find currency [{0}] in the database.", conversionRate.Conversion.To);
                throw new InvalidOperationException(message);
            }
            var buyRate = MoneyMath.Round(conversionRate.Rate * (decimal) Randomizer.FromRange(_settings.BuyRateCoefficientRange), rounding);
            var sellRate = MoneyMath.Round(conversionRate.Rate * (decimal) Randomizer.FromRange(_settings.SellRateCoefficientRange), rounding);
            return ExchangeRate.Create(baseCurrency, foreignCurrency, buyRate, sellRate, timstampUtc);
        }

        public ExchangeRate CalculateCrossRate(ExchangeRate rate1, ExchangeRate rate2, DateTime timestampUtc)
        {
            Argument.NotNull(rate1, "rate1");
            Argument.NotNull(rate2, "rate2");

            if (rate1.Base.ISOName != rate2.Base.ISOName)
            {
                throw new ArgumentException("Can't calculate cross rate for rates with different base currencies.");
            }
            if (rate1.Foreign.ISOName == rate2.Foreign.ISOName)
            {
                throw new ArgumentException("Can't calculate cross rate for rates with same foreign currencies.");
            }
            var buyRate = rate2.BuyRate / rate1.SellRate;
            var sellRate = rate2.SellRate / rate1.BuyRate;

            return ExchangeRate.Create(rate1.Foreign, rate2.Foreign, buyRate, sellRate, timestampUtc);
        }
    }    
}
