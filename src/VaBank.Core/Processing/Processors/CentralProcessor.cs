using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Factories;
using VaBank.Core.Processing.Processors.Abstract;

namespace VaBank.Core.Processing.Processors
{
    public class CentralProcessor
    {
        private readonly ProcessorFactory _processorFactory;

        public CentralProcessor(ProcessorFactory processorFactory)
        {
            Argument.NotNull(processorFactory, "processorFactory");
            _processorFactory = processorFactory;
        }

        public OperationProcessorResult Process(BankOperation operation)
        {
            var processor = _processorFactory.CreateFor(operation);
            return processor.Process(operation);
        }

        public TransactionProcessorResult Process(Transaction transaction, BankOperation operation = null)
        {
            var processor = _processorFactory.CreateFor(transaction, operation);
            return processor.Process(transaction);
        }
    }
}
