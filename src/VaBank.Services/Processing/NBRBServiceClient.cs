using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VaBank.Core.Processing;
using VaBank.Services.NBRBWebService;

namespace VaBank.Services.Processing
{
    internal class NBRBServiceClient : IDisposable
    {
        private bool _isDisposed;

        private readonly ExRatesSoapClient _client = new ExRatesSoapClient();

        private static class Columns
        {
            public const string ISOName = "Cur_Abbreviation";
            public const string Rate = "Cur_OfficialRate";
        }

        public IList<ConversionRate> GetLatestRates()
        {
            var rates = GetRateRows(DateTime.Now).Select(Parse).ToList();
            return rates;
        }

        private static ConversionRate Parse(DataRow row)
        {
            var conversion = new CurrencyConversion("BYR", (string) row[Columns.ISOName]);
            return new ConversionRate(conversion, (decimal) row[Columns.Rate]);
        }

        private IEnumerable<DataRow> GetRateRows(DateTime date)
        {
            return _client.ExRatesDaily(date).Tables[0].Rows.OfType<DataRow>();            
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _client.Close();
            _isDisposed = true;
        }
    }
}
