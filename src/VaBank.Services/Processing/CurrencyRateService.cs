using System;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Processing;
using VaBank.Services.Contracts.Processing.Models;
using VaBank.Services.Contracts.Processing.Queries;

namespace VaBank.Services.Processing
{
    public class CurrencyRateService : BaseService, ICurrencyRateService
    {
        private readonly IQueryRepository<CurrencyRate> _currencyRateRepository;
        
        public CurrencyRateService(BaseServiceDependencies dependencies,
            IQueryRepository<CurrencyRate> currencyRateRepository)
            : base(dependencies)
        {
            if (currencyRateRepository == null)
                throw new ArgumentNullException("currencyRateRepository");
            _currencyRateRepository = currencyRateRepository;
        }

        public void UpdateRates()
        {
            //TODO: Apply rates adjuster
            throw new NotImplementedException();
        }

        public CurrencyRatesLookupModel GetAllTodayRates()
        {
            return GetAllRates(DateTime.Today);
        }

        public CurrencyRatesLookupModel GetAllRates(DateTime date)
        {
            var rates =
                _currencyRateRepository.Project<CurrencyRateModel>(
                    DbQuery.For<CurrencyRate>().FilterBy(x => x.TimestampUtc.Date == date)).ToList();
            return new CurrencyRatesLookupModel { CurrencyRates = rates };
        }

        public CurrencyRateModel GetRate(CurrencyRateQuery query)
        {
            if (query == null)
                throw new ArgumentNullException("query");
            return
                _currencyRateRepository.ProjectOne<CurrencyRate, CurrencyRateModel>(
                    DbQuery.For<CurrencyRate>().FromClientQuery(query));
        }
    }
}
