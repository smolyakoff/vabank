using VaBank.Common.Data.Repositories;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Processors;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Processing.Events;

namespace VaBank.Jobs.Processing
{
    public class OperationProcessingJobContext : DefaultJobContext<IBankOperationEvent>
    {
        public IRepository<BankOperation> Operations { get; set; }

        public CentralProcessor Processor { get; set; }
    }
}
