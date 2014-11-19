using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Processing;
using VaBank.Services.Contracts.Processing.Events;

namespace VaBank.Jobs.Processing
{
    public class OperationProcessingJobContext : DefaultJobContext<IBankOperationEvent>
    {
        public IProcessingService ProcessingService { get; set; }
    }
}
