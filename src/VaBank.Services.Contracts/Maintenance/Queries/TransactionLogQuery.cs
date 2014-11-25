using System;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Paging;
using VaBank.Common.Data.Sorting;

namespace VaBank.Services.Contracts.Maintenance.Queries
{
    public class TransactionLogQuery : IClientFilterable, IClientSortable
    {
        public TransactionLogQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
            ClientSort = new DynamicLinqSort("CreatedDateUtc DESC");
        }

        public IFilter ClientFilter { get; set; }

        public Guid? UserId { get; set; }

        public ISort ClientSort { get; set; }
    }
}
