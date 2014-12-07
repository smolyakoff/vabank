using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Payments.Queries
{
    public class PaymentArchiveQuery : IClientFilterable
    {
        public PaymentArchiveQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
        }

        public IFilter ClientFilter { get; set; }
    }
}
