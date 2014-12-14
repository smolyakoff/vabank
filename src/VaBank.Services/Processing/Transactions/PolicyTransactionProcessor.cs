using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Processing.Events;
using VaBank.Services.Contracts.Processing.Models;
using VaBank.Services.Processing.Transactions.Policies;

namespace VaBank.Services.Processing.Transactions
{
    [Injectable]
    internal class PolicyTransactionProcessor : ITransactionProcessor
    {
        private readonly List<IPolicy> _policies;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public PolicyTransactionProcessor(IEnumerable<IPolicy> policies)
        {
            Argument.NotNull(policies, "policies");
            _policies = new List<IPolicy>(policies.OrderBy(x => x.Priority));
        }

        public IEnumerable<ApplicationEvent> Process(TransactionProcessorCommand command)
        {
            var policy = _policies.FirstOrDefault(x => x.AppliesTo(command.Transaction, command.BankOperation));
            if (command.Transaction.Status != ProcessStatus.Pending)
            {
                _logger.Warn("Transaction #{0} was passed to processor in invalid state.", command.Transaction.Id);
                return Enumerable.Empty<ApplicationEvent>();
            }
            if (policy == null)
            {
                throw new ArgumentException("Can't process transaction. No policy found.", "command");
            }
            var dynamicPolicy = (dynamic) policy;
            try
            {
                return Process(dynamicPolicy, command);
            }
            catch (Exception ex)
            {
                return OnFatalError(command, ex);
            }
        }
        

        public bool CanProcess(Transaction transaction, BankOperation operation)
        {
            return true;
        }

        private IEnumerable<ApplicationEvent> Process(CompletePolicy policy, TransactionProcessorCommand command)
        {
            command.Transaction.Complete(policy.GetPostDateUtc(command.Transaction, command.BankOperation));
            var bankOperationId = command.BankOperation == null ? null : (long?)command.BankOperation.Id;
            var operation = command.BankOperation == null
                ? null
                : command.BankOperation.ToModel<BankOperation, BankOperationModel>();
            var transaction = command.Transaction.ToModel<TransactionModel>();
            var events = new List<ApplicationEvent>()
            {
                new TransactionProcessedEvent(command.OperationId, transaction, bankOperationId)
            };
            if (operation != null)
            {
                events.Add(new OperationProgressEvent(command.OperationId, operation));
            }
            return events;
        }

        private IEnumerable<ApplicationEvent> Process(DisallowPolicy policy, TransactionProcessorCommand command)
        {
            command.Transaction.Fail(policy.GetErrorMessage(command.Transaction, command.BankOperation));
            var bankOperationId = command.BankOperation == null ? null : (long?)command.BankOperation.Id;
            var operation = command.BankOperation == null 
                ? null
                : command.BankOperation.ToModel<BankOperation, BankOperationModel>();
            var transaction = command.Transaction.ToModel<TransactionModel>();
            var events = new List<ApplicationEvent>()
            {
                new TransactionProcessedEvent(command.OperationId, transaction, bankOperationId)
            };
            if (operation != null)
            {
                events.Add(new OperationProgressEvent(command.OperationId, operation));
            }
            return events;
        }

        private IEnumerable<ApplicationEvent> Process(PostponePolicy policy, TransactionProcessorCommand command)
        {
            var transactionModel = command.Transaction.ToModel<TransactionModel>();
            var operationId = command.BankOperation == null ? null : (long?)command.BankOperation.Id;
            var @event = new TransactionProgressEvent(command.OperationId, transactionModel, operationId);
            return new List<ApplicationEvent>()
            {
                new PostponedEvent(@event, policy.GetScheduledDateUtc(command.Transaction, command.BankOperation))
            };
        }

        private IEnumerable<ApplicationEvent> OnFatalError(TransactionProcessorCommand command, Exception exception)
        {
            command.Transaction.Fail(Messages.UnknownTransactionError);
            var bankOperationId = command.BankOperation == null ? null : (long?)command.BankOperation.Id;
            var operation = command.BankOperation == null
                ? null
                : command.BankOperation.ToModel<BankOperation, BankOperationModel>();
            var transaction = command.Transaction.ToModel<TransactionModel>();
            var events = new List<ApplicationEvent>()
            {
                new TransactionProcessedEvent(command.OperationId, transaction, bankOperationId)
            };
            if (operation != null)
            {
                events.Add(new OperationProgressEvent(command.OperationId, operation));
            }
            return events;
        }
    }
}
