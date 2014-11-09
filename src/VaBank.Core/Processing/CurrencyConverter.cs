using System;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing
{
    public class CurrencyConverter
    {
        private readonly ExchangeRate _exchangeRate;

        internal CurrencyConverter(ExchangeRate exchangeRate)
        {
            Argument.NotNull(exchangeRate, "rate");
            _exchangeRate = exchangeRate;
        }

        public decimal Convert(CurrencyConversion conversion, decimal amount)
        {
            if (_exchangeRate.Key != conversion.ExchangeRateKey)
            {
                var message = string.Format("Conversion {0} is not supported.", conversion);
                throw new NotSupportedException(message);
            }
            var convertedAmount = _exchangeRate.Base.ISOName == conversion.From 
                ? ConvertFromBase(amount) 
                : ConvertFromForeign(amount);
            return convertedAmount;
        }

        private decimal ConvertFromBase(decimal amount)
        {
            return amount / _exchangeRate.SellRate;
        }

        private decimal ConvertFromForeign(decimal amount)
        {
            return amount * _exchangeRate.BuyRate;
        }
    }
}
