using System;
using VaBank.Common.Data.Filtering;
using VaBank.Services.Contracts.Common.Queries;

namespace VaBank.Services.Contracts.Payments.Queries
{
    public class PaymentArchiveQuery : IClientFilterable, IUserQuery
    {
        public PaymentArchiveQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
        }

        public Guid UserId { get; set; }

        public IFilter ClientFilter { get; set; }
    }
}
