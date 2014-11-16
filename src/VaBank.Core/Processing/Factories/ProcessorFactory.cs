using System;
using VaBank.Common.IoC;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Processors.Abstract;

namespace VaBank.Core.Processing.Factories
{
    [Injectable]
    public class ProcessorFactory
    {
        public IOperationProcessor<TOperation> CreateFor<TOperation>(TOperation operation)
            where TOperation : BankOperation
        {
            //TODO: think how to implement this: using ioc or no?
            throw new NotImplementedException();
        }

        public ITransactionProcessor<TTransaction, TOperation> CreateFor<TTransaction, TOperation>(TTransaction transaction, TOperation operation) 
            where TTransaction : Transaction 
            where TOperation : BankOperation
        {
            //TODO: think how to implement this: using ioc or no?
            throw new NotImplementedException();
        } 
    }
}
