using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
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

        private readonly ExchangeRateServiceSettings _settings;

        public ExchangeRateService(BaseServiceDependencies dependencies,
            ExchangeRateServiceDependencies exchangeRateServiceDependencies)
            : base(dependencies)
        {
            Argument.NotNull(exchangeRateServiceDependencies, "exchangeRateServiceDependencies");
            _deps = exchangeRateServiceDependencies;
            _settings = new ExchangeRateServiceSettings();
        }

        public IList<ExchangeRateModel> GetLocalCurrencyRates()
        {
            try
            {
                var rates = _deps.ExchangeRates.GetAllActual(_settings.NationalCurrencyISOName);
                return rates.Map<ExchangeRate, ExchangeRateModel>().ToList();
            }
            catch (Exception ex)
            {
                throw new ServerException("Can't get local currency rates.", ex);
            }
        }

        public void UpdateRates()
        {
            try
            {            
                var serviceClient = new NbrbServiceClient();
                var nbrbRates = serviceClient.GetTodayRates("USD", "EUR");

                //Update BYR to USD
                var usdRate = UpdateBaseBYR(_deps.Currencies.Find("USD"), nbrbRates.Single(x => x.ISOName == "USD"));
                //Update BYR to EUR
                var eurRate = UpdateBaseBYR(_deps.Currencies.Find("EUR"), nbrbRates.Single(x => x.ISOName == "EUR"));
                UpdateUSDToEUR(usdRate, eurRate);
                Commit();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't update currency rates.", ex);
            }
        }

        private ExchangeRate UpdateBaseBYR(Currency toCurrency, NBRBCurrencyRate nbrbRate)
        {
            var rate = _deps.ExchangeRateFactory.CalculateWithBYRBaseRate(toCurrency, nbrbRate.Rate);
            _deps.ExchangeRates.Save(rate);
            return rate;
        }

        private ExchangeRate UpdateUSDToEUR(ExchangeRate byrToUsd, ExchangeRate byrToEur)
        {
            var rate = _deps.ExchangeRateFactory.CalculateCrossRate(byrToUsd, byrToEur);
            _deps.ExchangeRates.Save(rate);
            return rate;
        }
    }
}
