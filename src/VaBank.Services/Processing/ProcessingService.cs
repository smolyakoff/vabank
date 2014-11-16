using System;
using MoreLinq;
using NLog;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Processing;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Processing
{
    public class ProcessingService : BaseService, IProcessingService
    {
        private readonly ProcessingServiceDependencies _deps;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ProcessingService(BaseServiceDependencies baseDepenendencies, ProcessingServiceDependencies processingServiceDependencies) 
            : base(baseDepenendencies)
        {
            Argument.NotNull(processingServiceDependencies, "processingServiceDependencies");
            processingServiceDependencies.EnsureIsResolved();

            _deps = processingServiceDependencies;
        }

        public BankOperationModel ProcessBankOperation(ProcessBankOperationCommand command)
        {
            try
            {
                var operation = _deps.BankOperations.SurelyFind(command.BankOperationId);
                var result = _deps.CentralProcessor.Process(operation);
                result.StartedTransactions.ForEach(_deps.Transactions.Create);
                Commit();
                if (result.RescheduledDateUtc != null)
                {
                    //publish operation rescheduled event
                    //Publish();
                }
                //TODO: 
                //foreach started transaction publish transaction started event
                return operation.ToModel<BankOperation, BankOperationModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't process bank operation.", ex);
            }
        }

        public TransactionModel ProcessTransaction(ProcessTransactionCommand command)
        {
            try
            {
                var transaction = _deps.Transactions.SurelyFind(command.TransactionId);
                var operation = _deps.BankOperations.Find(command.OperationId);
                var result = _deps.CentralProcessor.Process(transaction, operation);
                Commit();
                if (result.RescheduledDateUtc != null)
                {
                    //publish transaction rescheduled event
                    //Publish();
                }
                if (result.NotifyOperationProcessor)
                {
                    if (operation == null)
                    {
                        _logger.Error("Can't notify operation processor because transaction is not bound to operation.");
                    }
                    else if (result.RescheduledDateUtc != null)
                    {
                        _logger.Warn("No need to notify transaction processor. Transaction was rescheduled.");
                    }
                    else
                    {
                       //TODO: publish operation processor notification, so ProcessBankOperation will be called again
                    }
                }
                return transaction.ToModel<Transaction, TransactionModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't process transaction.", ex);
            }
        }
    }
}