using System;
using System.Data;
using Autofac;
using AutoMapper;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Processing.Events;

namespace VaBank.Jobs.Processing
{
    public class TransactionProcessingJob : EventListenerJob<TransactionProcessingJobContext, ITransactionEvent>
    {
        public TransactionProcessingJob(ILifetimeScope scope)
            : base(scope)
        {      
        }

        protected override void Execute(TransactionProcessingJobContext context)
        {
            var transaction = context.TransactionFactory.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                var command = Mapper.Map<ProcessTransactionCommand>(context.Data);
                context.CancellationToken.ThrowIfCancellationRequested();
                context.ProcessingService.ProcessTransaction(command);
                context.CancellationToken.ThrowIfCancellationRequested();
                transaction.Commit();
            }
            catch (ServiceException ex)
            {
                if (ex.TransactionRollback)
                {
                    transaction.Rollback();
                }
                OnError(context.Data, ex);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                OnError(context.Data, ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private void OnError(ITransactionEvent @event, Exception ex)
        {
            var message = string.Format("Error occured while processing operation #{0}.", @event.TransactionId);
            Logger.Error(message, ex);
        }
    }
}
