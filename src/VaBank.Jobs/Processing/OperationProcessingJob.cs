using Autofac;
using System;
using VaBank.Jobs.Common;
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
            var operation = context.Operations.Find(context.Data.BankOperationId);
            if (operation == null)
                throw new InvalidOperationException("Can't process bank operation, because it was deleted.");
            context.Processor.Process(operation);
        }
    }
}
