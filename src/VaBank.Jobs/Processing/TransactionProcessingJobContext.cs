using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Processing;
using VaBank.Services.Contracts.Processing.Events;

namespace VaBank.Jobs.Processing
{
    public class TransactionProcessingJobContext : DefaultJobContext<ITransactionEvent>
    {
        public IProcessingService ProcessingService { get; set; }
    }
}
