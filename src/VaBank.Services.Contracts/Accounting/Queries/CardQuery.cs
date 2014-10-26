using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Accounting.Queries
{
    public class CardQuery : IClientFilterable
    {
        public CardQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
        }

        public IFilter ClientFilter { get; set; }
    }
}
