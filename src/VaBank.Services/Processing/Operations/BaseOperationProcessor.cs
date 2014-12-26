using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Resources;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Processing.Events;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Processing.Operations
{
    public abstract class BaseOperationProcessor : IOperationProcessor
    {
        protected readonly Logger Logger;

        protected readonly MoneyConverter MoneyConverter;

        protected readonly TransactionReferenceBook TransactionReferenceBook;

        protected readonly IRepository<Transaction> TransactionRepository; 

        protected readonly BankSettings Settings;

        internal BaseOperationProcessor(BaseOperationProcessorDependencies baseDependencies)
        {
            Argument.NotNull(baseDependencies, "baseDependencies");
            baseDependencies.EnsureIsResolved();
            Logger = LogManager.GetLogger(GetType().FullName);

            MoneyConverter = baseDependencies.MoneyConverter;
            TransactionRepository = baseDependencies.TransactionRepository;
            TransactionReferenceBook = baseDependencies.TransactionReferenceBook;
            Settings = new BankSettings();
        }

        public IEnumerable<ApplicationEvent> Process(BankOperationProcessorCommand command)
        {
            Argument.NotNull(command, "command");
            var operation = command.BankOperation;
            if (operation.Status == ProcessStatus.Failed)
            {
                Logger.Warn("Operation #{0} has been already failed.", operation.Id);
                return Enumerable.Empty<ApplicationEvent>();
            }
            if (operation.Status == ProcessStatus.Completed)
            {
                Logger.Warn("Operation #{0} has been already completed.", operation.Id);
                return Enumerable.Empty<ApplicationEvent>();
            }
            var postponeDate = PostponeDateOrNull(operation);
            if (postponeDate != null)
            {
                var operationModel = command.BankOperation.ToModel<BankOperation, BankOperationModel>();
                var @event = new OperationProgressEvent(command.OperationId, operationModel);
                return new List<ApplicationEvent>
                {
                    new PostponedEvent(@event, postponeDate.Value)
                };
            }
            return ProcessPending(command);
        }

        public abstract bool CanProcess(BankOperation operation);

        protected abstract IEnumerable<ApplicationEvent> ProcessPending(BankOperationProcessorCommand command);

        protected abstract DateTime? PostponeDateOrNull(BankOperation operation);
    }
}
