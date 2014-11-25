using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Sorting;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Maintenance.Queries
{
    public class TransactionLogQuery : IClientFilterable, IClientSortable
    {
        public TransactionLogQuery()
        {
            Status = new ProcessStatusModel[0];
            ClientFilter = new AlwaysTrueFilter();
            ClientSort = new DynamicLinqSort("CreatedDateUtc DESC");
        }

        public IFilter ClientFilter { get; set; }

        public ISort ClientSort { get; set; }

        public ProcessStatusModel[] Status { get; set; } 
    }
}
