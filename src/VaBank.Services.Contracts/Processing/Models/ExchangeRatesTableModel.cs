using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Validation;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class ExchangeRatesTableModel
    {
        public ExchangeRatesTableModel(IList<ExchangeRateModel> rates)
        {
            Argument.NotNull(rates, "rates");
            Argument.Satisfies(rates, x => x.Count > 0, "rates");

            Rates = rates;
        }

        public IList<ExchangeRateModel> Rates { get; private set; }

        public DateTime LastUpdatedUtc
        {
            get { return Rates.Select(x => x.TimestampUtc).OrderBy(x => x).First(); }
        }
    }
}
