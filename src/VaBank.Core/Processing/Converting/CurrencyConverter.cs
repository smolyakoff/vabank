using System;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing.Converting
{
    [Injectable]
    public class CurrencyConverter
    {
        private readonly IQueryRepository<CurrencyRate> _currencyRateRepository;

        public CurrencyConverter(IQueryRepository<CurrencyRate> currencyRateRepository)
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
                _currencyRateRepository.QueryOne(DbQuery.For<CurrencyRate>().FilterBy(x =>
                    x.From.ISOName == converting.From.ISOName && x.To.ISOName == converting.To.ISOName &&
                    x.TimestampUtc.Date == rateDate));
            var resultAmount = string.CompareOrdinal(converting.From.ISOName, converting.To.ISOName) > 1
                ? converting.Amount*currencyRate.BuyRate
                : converting.Amount*currencyRate.SellRate;

            return new CurrencyConvertingResult
            {
                Amount = resultAmount,
                Currency = converting.To
            };
        }
    }
}
