using System;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly ExchangeRate _exchangeRate;

        internal CurrencyConverter(ExchangeRate exchangeRate)
        {
            Argument.NotNull(exchangeRate, "rate");
            _exchangeRate = exchangeRate;
        }

        internal decimal Convert(CurrencyConversion conversion, decimal amount, out Currency resultingCurrency)
        {
            if (_exchangeRate.Key != conversion.ExchangeRateKey)
            {
                var message = string.Format("Conversion {0} is not supported.", conversion);
                throw new NotSupportedException(message);
            }
            var convertedAmount = _exchangeRate.Base.ISOName == conversion.From 
                ? ConvertFromBase(amount) 
                : ConvertFromForeign(amount);
            resultingCurrency = _exchangeRate.Base.ISOName == conversion.From
                ? _exchangeRate.Foreign
                : _exchangeRate.Base;
            return convertedAmount;
        }

        public decimal ConvertFromBase(decimal amount)
        {
            return amount / _exchangeRate.SellRate;
        }

        public decimal ConvertFromForeign(decimal amount)
        {
            return amount * _exchangeRate.BuyRate;
        }
    }
}
