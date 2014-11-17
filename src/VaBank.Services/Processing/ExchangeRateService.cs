using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using MoreLinq;
using VaBank.Common.Validation;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Processing;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Processing
{
    public class ExchangeRateService : BaseService, ICurrencyRateService
    {
        private readonly ExchangeRateServiceDependencies _deps;

        private readonly ExchangeRateSettings _settings;

        public ExchangeRateService(BaseServiceDependencies dependencies,
            ExchangeRateServiceDependencies exchangeRateServiceDependencies)
            : base(dependencies)
        {
            exchangeRateServiceDependencies.EnsureIsResolved();
            _deps = exchangeRateServiceDependencies;
            _settings = new ExchangeRateSettings();
        }

        public IList<ExchangeRateModel> GetLocalCurrencyRates()
        {
            try
            {
                var rates = _deps.ExchangeRates.GetAllActual(_settings.NationalCurrency);
                return rates.Map<ExchangeRate, ExchangeRateModel>().ToList();
            }
            catch (Exception ex)
            {
                throw new ServerException("Can't get local currency rates.", ex);
            }
        }

        public void UpdateRates()
        {
            var nationalBankClient = new NBRBServiceClient();
            try
            {
                var now = DateTime.UtcNow;
                var currencies = _deps.Currencies.FindAll();
                var nationalCurrency = currencies.First(x => x.ISOName == _settings.NationalCurrency);
                var foreignCurrencies = currencies.Except(new[] {nationalCurrency}).ToList();

                var nationalBankRates = nationalBankClient.GetLatestRates()
                    .Where(x => foreignCurrencies.Any(f => f.ISOName == x.Conversion.To))
                    .ToList();
                var commercialRates = nationalBankRates
                    .Select(x => _deps.ExchangeRateCalculator.CalculateFromNationalBankRate(x.Conversion.To, x.Rate, now))
                    .ToDictionary(x => x.Foreign.ISOName, x => x);

                var foreignCurrenciesIds = foreignCurrencies.Select(x => x.ISOName).ToList();
                var pairs = from id1 in foreignCurrenciesIds
                    from id2 in foreignCurrenciesIds
                    select new {id1, id2};
                var crossConversions = pairs.Where(x => x.id1 != x.id2)
                    .Select(x => new CurrencyConversion(x.id1, x.id2))
                    .OrderBy(x => _settings.GetPriority(x.From))
                    .DistinctBy(x => x.ExchangeRateKey)
                    .ToList();
                var crossRates = crossConversions
                    .Select(x => _deps.ExchangeRateCalculator.CalculateCrossRate(commercialRates[x.From],commercialRates[x.To], now))
                    .ToList();

                var allRates = commercialRates.Values.Concat(crossRates).ToList();
                allRates.ForEach(_deps.ExchangeRates.Save);
                Commit();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't update currency rates.", ex);
            }
            finally
            {
                nationalBankClient.Dispose();
            }
        }
    }
}
