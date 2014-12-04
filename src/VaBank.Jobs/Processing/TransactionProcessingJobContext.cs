using VaBank.Common.Data.Database;
using VaBank.Common.Events;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Processing;
using VaBank.Services.Contracts.Processing.Events;

namespace VaBank.Jobs.Processing
{
    public class TransactionProcessingJobContext : DefaultJobContext<TransactionProgressEvent>, ITransactionalJobContext
    {
        public IProcessingService ProcessingService { get; set; }
        public ITransactionFactory TransactionFactory { get; set; }

        public ISendOnlyServiceBus ServiceBus { get; set; }
    }
}
