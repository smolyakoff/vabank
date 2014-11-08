using System;
using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Processing.Queries
{
    public class CurrencyRateQuery : IClientFilterable
    {
        public CurrencyRateQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
        }
        
        public IFilter ClientFilter { get; set; }
    }
}
