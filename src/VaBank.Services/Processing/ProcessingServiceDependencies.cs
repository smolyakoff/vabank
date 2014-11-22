using VaBank.Common.Data.Repositories;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;

namespace VaBank.Services.Processing
{
    public class ProcessingServiceDependencies : IDependencyCollection
    {
        public IRepository<BankOperation> BankOperations { get; set; }

        public IRepository<Transaction> Transactions { get; set; } 
 
        public CentralProcessor CentralProcessor { get; set; }
    }
}
