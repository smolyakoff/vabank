using System;
using System.Linq;
using System.Runtime.Remoting;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
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

        public ExchangeRatesTableModel GetLocalCurrencyRates()
        {
            try
            {
                var rates = _deps.ExchangeRates.GetAllActual(_settings.NationalCurrencyISOName);
                return new ExchangeRatesTableModel(rates.Map<ExchangeRate, ExchangeRateModel>().ToList());
            }
            catch (Exception ex)
            {
                throw new ServerException("Can't get local currency rates.", ex);
            }
        }

        public void UpdateRates()
        {
            //TODO: Apply rates adjuster
            throw new NotImplementedException();
        }
    }
}
