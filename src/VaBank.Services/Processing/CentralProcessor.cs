using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Events;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Processing.Operations;
using VaBank.Services.Processing.Transactions;

namespace VaBank.Services.Processing
{
    [Injectable]
    public class CentralProcessor : IProcessor<BankOperationProcessorCommand>, IProcessor<TransactionProcessorCommand>
    {
        private readonly List<IOperationProcessor> _operationProcessors;

        private readonly List<ITransactionProcessor> _transactionProcessors; 

        public CentralProcessor(IEnumerable<IOperationProcessor> operationProcessors, IEnumerable<ITransactionProcessor> transactionProcessors)
        {
            Argument.NotNull(operationProcessors, "operationProcessors");
            Argument.NotNull(transactionProcessors, "transactionProcessors");
            _operationProcessors = operationProcessors.ToList();
            _transactionProcessors = transactionProcessors.ToList();
        }

        public IEnumerable<ApplicationEvent> Process(BankOperationProcessorCommand command)
        {
            var processor = _operationProcessors.FirstOrDefault(x => x.CanProcess(command.BankOperation));
            if (processor == null)
            {
                throw new ArgumentException("Can't process operation. No processor found.", "operation");
            }
            return processor.Process(command);
        }

        public IEnumerable<ApplicationEvent> Process(TransactionProcessorCommand command)
        {
            var processor = _transactionProcessors.FirstOrDefault(x => x.CanProcess(command.Transaction, command.BankOperation));
            if (processor == null)
            {
                throw new ArgumentException("Can't process operation. No processor found.", "command");
            }
            return processor.Process(command);
        }
    }
}
