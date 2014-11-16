using System;
using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Maintenance.Queries
{
    public class TransactionLogQuery : IClientFilterable
    {
        public TransactionLogQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
        }

        public IFilter ClientFilter { get; set; }

        public Guid? UserId { get; set; }
    }
}
