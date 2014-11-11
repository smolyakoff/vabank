using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VaBank.Services.NBRBWebService;

namespace VaBank.Services.Processing
{
    internal class NbrbServiceClient
    {
        private readonly ExRatesSoapClient _client = new ExRatesSoapClient();

        public IList<NBRBCurrencyRate> GetAllRates(DateTime date)
        {
            return GetCurrencyRatesRows(date).Select(x => ToCurrencyRate(x, date)).ToList();
        }

        public IList<NBRBCurrencyRate> GetRates(DateTime date, params string[] currencyISONames)
        {
            return
                GetCurrencyRatesRows(date)
                    .Where(x => currencyISONames.Contains(x[Names.ISOName]))
                    .Select(x => ToCurrencyRate(x, date))
                    .ToList();
        }

        public IList<NBRBCurrencyRate> GetAllTodayRates()
        {
            return GetAllRates(DateTime.Today);
        }

        public IList<NBRBCurrencyRate> GetTodayRates(params string[] currencyISONames)
        {
            return GetRates(DateTime.Today, currencyISONames);
        }

        private NBRBCurrencyRate ToCurrencyRate(DataRow row, DateTime date)
        {
            return new NBRBCurrencyRate
            {
                ISOName = (string) row[Names.ISOName], 
                Rate = (decimal) row[Names.Rate],
                Date = date
            };
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
