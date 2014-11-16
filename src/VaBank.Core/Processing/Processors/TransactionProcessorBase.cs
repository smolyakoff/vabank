using System;
using NLog;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Processors.Abstract;

namespace VaBank.Core.Processing.Processors
{
    public class TransactionProcessorBase<TTransaction, TOperation> : ITransactionProcessor<TTransaction, TOperation> 
        where TTransaction : Transaction 
        where TOperation : BankOperation
    {
        protected readonly Logger Logger;

        public TransactionProcessorBase()
        {
            Logger = LogManager.GetLogger(GetType().FullName);
        }

        public TransactionProcessorResult Process(TTransaction transaction, TOperation operation = null)
        {
            //the same logic as in operation processor
            throw new NotImplementedException();
        }
    }
}
