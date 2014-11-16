using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Factories;
using VaBank.Core.Processing.Repositories;

namespace VaBank.Core.Processing
{
    [Injectable]
    public class MoneyConverter
    {
        private readonly CurrencyConverterFactory _converterFactory;

        public MoneyConverter(IExchangeRateRepository exchangeRateRepository)
        {
            Argument.NotNull(exchangeRateRepository, "exchangeRateRepository");

            _converterFactory = new CurrencyConverterFactory(exchangeRateRepository);
        }

        public Money Convert(Money money, string destinationCurrencyISOName)
        {
            Argument.NotEmpty(destinationCurrencyISOName, "destinationCurrencyISOName");

            if (money.Currency.ISOName == destinationCurrencyISOName)
            {
                return new Money(money.Currency, money.Amount);
            }
            var conversion = new CurrencyConversion(money.Currency.ISOName, destinationCurrencyISOName);
            var converter = _converterFactory.Create(conversion);
            Currency resultingCurrency;
            var convertedAmount = converter.Convert(conversion, money.Amount, out resultingCurrency);
            return new Money(resultingCurrency, convertedAmount);
        }
    }
}
