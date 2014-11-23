﻿using System;
using System.Data;
using Autofac;
using AutoMapper;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Common;
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
            var transaction = context.TransactionFactory.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                var command = Mapper.Map<ProcessBankOperationCommand>(context.Data);
                context.CancellationToken.ThrowIfCancellationRequested();
                context.ProcessingService.ProcessBankOperation(command);
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

        private void OnError(IBankOperationEvent @event, Exception ex)
        {
            var message = string.Format("Error occured while processing operation #{0}.", @event.BankOperationId);
            Logger.Error(message, ex);
        }
    }
}