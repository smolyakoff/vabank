using System;
using VaBank.Common.IoC;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.App.Repositories;

namespace VaBank.Core.Accounting.Factories
{
    [Injectable]
    public class CardLimitsFactory
    {
        private const string DefaultLimitsKey = "VaBank.Accounting.CardLimits.Default.{0}";

        private readonly ISettingRepository _settingRepository;

        public CardLimitsFactory(ISettingRepository settingRepository)
        {
            if (settingRepository == null)
            {
                throw new ArgumentNullException("settingRepository");
            }
            _settingRepository = settingRepository;
        }

        public CardLimits CreateDefault(Currency currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }
            var key = string.Format(DefaultLimitsKey, currency.ISOName);
            var limits = _settingRepository.Get<CardLimits>(key);
            if (limits == null)
            {
                var message = string.Format("No default limits found [{0}].", currency.ISOName);
                throw new InvalidOperationException(message);
            }
            return limits;
        }
    }
}
