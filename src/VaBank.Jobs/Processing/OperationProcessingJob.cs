using Autofac;
using AutoMapper;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Processing.Events;

namespace VaBank.Jobs.Processing
{
    [JobName("OperationProcessing")]
    public class OperationProcessingJob : EventListenerJob<OperationProcessingJobContext, IBankOperationEvent>
    {
        public OperationProcessingJob(ILifetimeScope scope)
            : base(scope)
        {
        }
        
        protected override void Execute(OperationProcessingJobContext context)
        {  
            context.ProcessingService.ProcessBankOperation(Mapper.Map<ProcessBankOperationCommand>(context.Data));
        }
    }
}
