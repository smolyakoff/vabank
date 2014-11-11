using System;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing
{    
    [Injectable]
    public class ExchangeRateFactory
    {
        private Random _rand;
        private readonly IRepository<Currency> _currencyRepository;

        public ExchangeRateFactory(IRepository<Currency> currencyRepository)
        {
            Argument.NotNull(currencyRepository, "currencyRepository");

            _currencyRepository = currencyRepository;
        }

        private Random Random { get { return _rand ?? (_rand = new Random()); } }

        private RateCalculation Calculate(Currency baseCurrency, Currency foreignCurrency, decimal baseRate)
        {
            var buyRate = baseRate * (decimal)GetFactorFromInterval(Factors.BuyLowerFactor, Factors.BuyUpperFactor);
            var sellRate = baseRate * (decimal)GetFactorFromInterval(Factors.SellLowerFactor, Factors.SellUpperFactor);

            return new RateCalculation
            {
                BaseCurrency = baseCurrency,
                ForeignCurrency = foreignCurrency,
                BuyRate = buyRate,
                SellRate = sellRate,
                TimestampUtc = DateTime.UtcNow
            };
        }

        public ExchangeRate CalculateRate(Currency baseCurrency, Currency foreignCurrency, decimal baseRate)
        {
            return Calculate(baseCurrency, foreignCurrency, baseRate).ToExchangeRate();
        }

        public ExchangeRate CalculateWithBYRBaseRate(Currency foreignCurrency, decimal baseRate)
        {
            Argument.NotNull(foreignCurrency, "foreignCurrency");

            var byr = _currencyRepository.Find("BYR");
            if (byr == null)
                throw new InvalidOperationException("Can't create rate exchange, because currency BYR doesn't exist at a database.");
            var calculation = Calculate(byr, foreignCurrency, baseRate);
            RoundRates(calculation, 10);

            return calculation.ToExchangeRate();
        }

        public ExchangeRate CalculateCrossRate(ExchangeRate baseRate, ExchangeRate foreignRate)
        {
            Argument.NotNull(baseRate, "baseRate");
            Argument.NotNull(foreignRate, "foreignRate");
            
            if (baseRate.Base.ISOName != foreignRate.Base.ISOName)
                throw new InvalidOperationException("Can't calculate cross exchange rate with different base currencies");

            var buyRate = baseRate.BuyRate / foreignRate.SellRate;
            var sellRate = baseRate.SellRate / foreignRate.BuyRate;

            return ExchangeRate.Create(baseRate.Foreign, foreignRate.Foreign, buyRate, sellRate, DateTime.UtcNow);
        }

        private double GetFactorFromInterval(double lower, double upper)
        {
            if (lower > upper)
                throw new ArgumentOutOfRangeException("lower", "Lower value can't be less than upper.");

            return lower + (upper - lower) * Random.NextDouble();
        }

        private void RoundRates(RateCalculation rate, int multiplicity)
        {
            if (multiplicity < 2)
                throw new ArgumentOutOfRangeException("multiplicity");
            rate.BuyRate = RoundRate(rate.BuyRate, multiplicity);
            rate.SellRate = RoundRate(rate.SellRate, multiplicity);
        }

        private decimal RoundRate(decimal rate, int multiplicity)
        {  
            return Math.Ceiling(rate/multiplicity)*multiplicity;
        }        

        private class Factors
        {
            public const double BuyLowerFactor = 0.99;
            public const double BuyUpperFactor = 1;
            public const double SellLowerFactor = 1.006;
            public const double SellUpperFactor = 1.01;
        }

        private class RateCalculation
        {
            public Currency BaseCurrency { get; set; }
            public Currency ForeignCurrency { get; set; }
            public decimal BuyRate { get; set; }
            public decimal SellRate { get; set; }
            public DateTime TimestampUtc { get; set; }

            public ExchangeRate ToExchangeRate()
            {
                return ExchangeRate.Create(BaseCurrency, ForeignCurrency, BuyRate, SellRate, TimestampUtc);
            }
        }
    }    
}
