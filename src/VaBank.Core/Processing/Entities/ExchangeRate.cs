using System;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Common;
using VaBank.Core.Processing.Repositories;

namespace VaBank.Core.Processing.Entities
{
    public class ExchangeRate : Entity<Guid>
    {
        private readonly Lazy<ExchangeRateKey> _key; 

        private CurrencyConverter _converter;

        public static ExchangeRate Create(
            Currency baseCurrency,
            Currency foreignCurrency,
            decimal buyRate,
            decimal sellRate,
            DateTime timestampUtc)
        {
            Argument.NotNull(baseCurrency, "baseCurrency");
            Argument.NotNull(foreignCurrency, "foreignCurrency");
            Argument.Satisfies(foreignCurrency, x => x.ISOName == baseCurrency.ISOName, "foreignCurrency", "Can't create rate to same currency.");
            Argument.Satisfies(buyRate, x => x > 0, "buyRate");
            Argument.Satisfies(sellRate, x => x > 0, "sellRate");

            return new ExchangeRate(baseCurrency, foreignCurrency, buyRate, sellRate, timestampUtc);
        }

        private ExchangeRate(
            Currency baseCurrency, 
            Currency foreignCurrency, 
            decimal buyRate,
            decimal sellRate,
            DateTime timestampUtc)
        {
            Id = Guid.NewGuid();
            TimestampUtc = timestampUtc;
            Base = baseCurrency;
            Foreign = foreignCurrency;
            BuyRate = buyRate;
            SellRate = sellRate;
            IsActual = true;
            _key = new Lazy<ExchangeRateKey>(() => new ExchangeRateKey(Base.ISOName, Foreign.ISOName));
        }

        protected ExchangeRate()
        {
            _key = new Lazy<ExchangeRateKey>(() => new ExchangeRateKey(Base.ISOName, Foreign.ISOName));
        }

        public ExchangeRateKey Key
        {
            get { return _key.Value; }
        }

        public virtual Currency Base { get; protected set; }

        public virtual Currency Foreign { get; protected set; }

        public decimal BuyRate { get; protected set; }

        public decimal SellRate { get; protected set; }

        public DateTime TimestampUtc { get; protected set; }

        public bool IsActual { get; protected set; }

        public CurrencyConverter Converter
        {
            get { return _converter ?? (_converter = new CurrencyConverter(this)); }
        }
    }
}
