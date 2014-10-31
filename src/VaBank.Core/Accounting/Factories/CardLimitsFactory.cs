using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Util;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.App.Repositories;

namespace VaBank.Core.Accounting.Factories
{
    [Injectable]
    public class CardLimitsFactory
    {
        private const string DefaultLimitsKey = "VaBank.Accounting.CardLimits.Default.{0}";
        private const string RangeLimitsKey = "VaBank.Accounting.CardLimits.Range.{0}";

        private readonly ISettingRepository _settingRepository;
        private readonly IRepository<Currency> _currencyRepository;

        public CardLimitsFactory(ISettingRepository settingRepository, IRepository<Currency> currencyRepository)
        {
            Assert.NotNull("settingRepository", settingRepository);
            Assert.NotNull("currencyRepository", currencyRepository);
            _settingRepository = settingRepository;
            _currencyRepository = currencyRepository;
        }

        public CardLimits CreateDefault(Currency currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }
            var key = string.Format(DefaultLimitsKey, currency.ISOName);
            var limits = _settingRepository.GetOrDefault<CardLimits>(key);
            if (limits == null)
            {
                var message = string.Format("No default limits found [{0}].", currency.ISOName);
                throw new InvalidOperationException(message);
            }
            return limits;
        }

        public Dictionary<string, CardLimitsRange> FindAllRanges()
        {
            var currencies = _currencyRepository.FindAll();
            var keys = currencies
                .Select(x => x.ISOName)
                .Select(x => string.Format(RangeLimitsKey, x))
                .ToArray();
            return _settingRepository.BatchGet<CardLimitsRange>(keys);
        }  

        public CardLimitsRange FindRange(string currencyIsoName)
        {
            Assert.NotEmpty("currencyIsoName", currencyIsoName);
            var key = string.Format(RangeLimitsKey, currencyIsoName);
            var limits = _settingRepository.GetOrDefault<CardLimitsRange>(key);
            if (limits == null)
            {
                var message = string.Format("No range limits found [{0}].", currencyIsoName);
                throw new InvalidOperationException(message);
            }
            return limits;
        }
    }
}
