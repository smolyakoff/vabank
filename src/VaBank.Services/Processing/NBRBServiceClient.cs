using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VaBank.Core.Processing.Entities;
using VaBank.Services.NBRBWebService;

namespace VaBank.Services.Processing
{
    internal class NbrbServiceClient
    {
        private readonly ExRatesSoapClient _client = new ExRatesSoapClient();
        
        public IList<CurrencyRate> GetAllRates(DateTime date)
        {
            return GetCurrencyRatesRows(date).Select(x => ToCurrencyRate(x, date)).ToList();
        }

        public IList<CurrencyRate> GetTodayRates(DateTime date, params string[] currencyISONames)
        {
            return
                GetCurrencyRatesRows(date)
                    .Where(x => currencyISONames.Contains(x[Names.ISOName]))
                    .Select(x => ToCurrencyRate(x, date))
                    .ToList();
        }

        public IList<CurrencyRate> GetAllTodayRates()
        {
            return GetAllRates(DateTime.Today);
        }

        public IList<CurrencyRate> GetTodayRates(params string[] currencyISONames)
        {
            return GetTodayRates(DateTime.Today, currencyISONames);
        }

        private CurrencyRate ToCurrencyRate(DataRow row, DateTime date)
        {
            return CurrencyRate.Create((string) row[Names.ISOName], "BYR", (decimal) row[Names.Rate], date);
        }

        private IEnumerable<DataRow> GetCurrencyRatesRows(DateTime date)
        {
            return _client.ExRatesDaily(date).Tables[0].Rows.OfType<DataRow>();            
        }

        private static class Names
        {
            public const string ISOName = "Cur_Abbreviation";
            public const string Rate = "Cur_OfficialRate";
        }
    }
}
