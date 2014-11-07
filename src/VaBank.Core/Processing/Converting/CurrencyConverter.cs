using System;
using VaBank.Common.IoC;
using VaBank.Core.Processing.Repositories;

namespace VaBank.Core.Processing.Converting
{
    [Injectable]
    public class CurrencyConverter
    {
        private readonly ICurrencyRateRepository _currencyRateRepository;

        public CurrencyConverter(ICurrencyRateRepository currencyRateRepository)
        {
            if (_currencyRateRepository == null)
                throw new ArgumentNullException("currencyRateRepository");
            _currencyRateRepository = currencyRateRepository;
        }

        public CurrencyConvertingResult ConvertWithTodayRate(CurrencyConverting converting)
        {
            return ConvertWithRate(converting, DateTime.Today);           
        }

        public CurrencyConvertingResult ConvertWithRate(CurrencyConverting converting, DateTime rateDate)
        {
            if (converting == null)
                throw new ArgumentNullException("converting");
            if (converting.From.ISOName == converting.To.ISOName)
                throw new InvalidOperationException("Can't convert currency to same type.");

            var currencyRate =
                _currencyRateRepository.GetRate(new CurrencyNamePair(converting.To.ISOName, converting.From.ISOName),
                    rateDate);
            var resultAmount = string.CompareOrdinal(converting.From.ISOName, converting.To.ISOName) > 1
                ? converting.Amount*currencyRate.BuyingRate
                : converting.Amount*currencyRate.SellingRate;

            return new CurrencyConvertingResult
            {
                Amount = resultAmount,
                Currency = converting.To
            };
        }
    }
}
