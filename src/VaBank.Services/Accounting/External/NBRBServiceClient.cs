using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VaBank.Core.Accounting.Entities;
using VaBank.Services.NBRBWebService;

namespace VaBank.Services.Accounting.External
{
    public static class NbrbServiceClient
    {
        public static IList<CurrencyRate> GetAllRates(DateTime date)
        {
            return GetCurrencyRatesRows(date).Select(x => ToCurrencyRate(x, date)).ToList();
        }

        public static IList<CurrencyRate> GetTodayRates(DateTime date, params string[] currencyISONames)
        {
            return
                GetCurrencyRatesRows(date)
                    .Where(x => currencyISONames.Contains(x[Names.ISOName]))
                    .Select(x => ToCurrencyRate(x, date))
                    .ToList();
        }

        public static IList<CurrencyRate> GetAllTodayRates()
        {
            return GetAllRates(DateTime.Today);
        }

        public static IList<CurrencyRate> GetTodayRates(params string[] currencyISONames)
        {
            return GetTodayRates(DateTime.Today, currencyISONames);
        }

        private static CurrencyRate ToCurrencyRate(DataRow row, DateTime date)
        {
            return CurrencyRate.Create((string) row[Names.ISOName], "BYR", (decimal) row[Names.Rate], date);
        }

        private static IEnumerable<DataRow> GetCurrencyRatesRows(DateTime date)
        {
            var client = new ExRatesSoapClient();
            return client.ExRatesDaily(date).Tables[0].Rows.OfType<DataRow>();
        }

        private static class Names
        {
            public const string ISOName = "Cur_Abbreviation";
            public const string Rate = "Cur_OfficialRate";
        }
    }
}
