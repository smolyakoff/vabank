using VaBank.Common.Data.Repositories;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Resources;
using VaBank.Services.Common;

namespace VaBank.Services.Processing.Operations
{
    internal class BaseOperationProcessorDependencies : IDependencyCollection
    {
        public MoneyConverter MoneyConverter { get; set; }

        public TransactionReferenceBook TransactionReferenceBook { get; set; }

        public IRepository<Transaction> TransactionRepository { get; set; } 
    }
}
