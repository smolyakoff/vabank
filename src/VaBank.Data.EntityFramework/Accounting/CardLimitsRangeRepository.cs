using System;
using System.Collections.Generic;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Accounting.Repositories;
using VaBank.Core.App.Repositories;

namespace VaBank.Data.EntityFramework.Accounting
{
    public class CardLimitsRangeRepository : ICardLimitsRangeRepository
    {
        private const string Key = "VaBank.Accounting.CardLimits.Range.{0}";

        private readonly ISettingRepository _settingRepository;
        private readonly IRepository<Currency> _currencyRepository;

        public CardLimitsRangeRepository(IRepository<Currency> currencyRepository, ISettingRepository settingRepository)
        {
            if (currencyRepository == null)
                throw new ArgumentNullException("currencyRepository");
            if (settingRepository == null)
                throw new ArgumentNullException("settingRepository");
            _currencyRepository = currencyRepository;
            _settingRepository = settingRepository;
        }

        public IList<CardLimitsRange> GetAll()
        {
            var limits = new List<CardLimitsRange>();
            foreach (var currency in _currencyRepository.FindAll())
            {
                var key = string.Format(Key, currency.ISOName);
                var limit = _settingRepository.Get<CardLimitsRange>(key);
                if (limit != null)
                    limits.Add(limit);
            }
            return limits;
        }

        public CardLimitsRange GetWithISOName(string isoName)
        {
            var key = string.Format(Key, isoName);
            var limit = _settingRepository.Get<CardLimitsRange>(key);
            return limit;
        }
    }
}
