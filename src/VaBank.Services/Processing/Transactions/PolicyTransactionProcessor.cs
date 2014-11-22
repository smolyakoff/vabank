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
            if (policy == null)
            {
                throw new ArgumentException("Can't process transaction. No policy found.", "command");
            }
            var dynamicPolicy = (dynamic) policy;
            return Process(dynamicPolicy, command);
        }

        public bool CanProcess(Transaction transaction, BankOperation operation)
        {
            return true;
        }

        private IEnumerable<ApplicationEvent> Process(CompletePolicy policy, TransactionProcessorCommand command)
        {
            command.Transaction.Complete(policy.GetPostDateUtc(command.Transaction, command.BankOperation));
            if (command.BankOperation == null)
            {
                return Enumerable.Empty<ApplicationEvent>();
            }
            var operation = command.BankOperation.ToModel<BankOperation, BankOperationModel>();
            return new List<ApplicationEvent>()
            {
                new OperationProgressEvent(command.OperationId, operation)
            };
        }

        private IEnumerable<ApplicationEvent> Process(DisallowPolicy policy, TransactionProcessorCommand command)
        {
            command.Transaction.Fail(policy.GetErrorMessage(command.Transaction, command.BankOperation));
            if (command.BankOperation == null)
            {
                return Enumerable.Empty<ApplicationEvent>();
            }
            var operation = command.BankOperation.ToModel<BankOperation, BankOperationModel>();
            return new List<ApplicationEvent>()
            {
                new OperationProgressEvent(command.OperationId, operation)
            };
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
    }
}
