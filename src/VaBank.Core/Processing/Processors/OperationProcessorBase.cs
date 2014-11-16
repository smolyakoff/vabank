using NLog;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Processors.Abstract;

namespace VaBank.Core.Processing.Processors
{
    public abstract class OperationProcessorBase<TOperation> : IOperationProcessor<TOperation> 
        where TOperation : BankOperation
    {
        protected readonly Logger Logger;

        protected OperationProcessorBase()
        {
            Logger = LogManager.GetLogger(GetType().FullName);
        } 

        public OperationProcessorResult Process(TOperation operation)
        {
            Argument.NotNull(operation, "operation");
            if (operation.Status == ProcessStatus.Failed)
            {
                //warn some message here
                Logger.Warn("");
                return OperationProcessorResult.Empty();
            }
            if (operation.Status == ProcessStatus.Completed)
            {
                //warn some message
                Logger.Warn("");
                return OperationProcessorResult.Empty();
            }
            return ProcessPending(operation);
        }

        protected abstract OperationProcessorResult ProcessPending(TOperation operation);
    }
}
