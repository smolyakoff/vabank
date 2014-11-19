using Autofac;
using AutoMapper;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Processing.Events;

namespace VaBank.Jobs.Processing
{
    public class TransactionProcessingJob : EventListenerJob<TransactionProcessingJobContext, ITransactionEvent>
    {
        public TransactionProcessingJob(ILifetimeScope scope)
            : base(scope)
        { }

        protected override void Execute(TransactionProcessingJobContext context)
        {
            context.ProcessingService.ProcessTransaction(Mapper.Map<ProcessTransactionCommand>(context.Data));
        }
    }
}
