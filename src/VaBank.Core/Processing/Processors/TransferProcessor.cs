using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Processors.Abstract;

namespace VaBank.Core.Processing.Processors
{
    public class TransferProcessor<TTransfer> : OperationProcessorBase<TTransfer>
        where TTransfer : Transfer
    {
        protected override OperationProcessorResult ProcessPending(TTransfer operation)
        {
            //choose state and go on :)
            throw new NotImplementedException();
        }

        private IReadOnlyCollection<Transaction> WhenNoTransactionsStarted(Transfer transfer)
        {
            //create only the first transaction: withdraw money from first account!
            throw new NotImplementedException();
        }

        private IReadOnlyCollection<Transaction> WhenFirstTransactionCompleted(Transfer transfer)
        {
            //create second transaction (deposit to second account)
            throw new NotImplementedException();
        }

        private IReadOnlyCollection<Transaction> WhenFirstTransactionFaulted(Transfer transfer)
        {
            //hey, don't forget to return money to the first account!
            throw new NotImplementedException();
        }

        private IReadOnlyCollection<Transaction> WhenSecondTransactionCompleted(Transfer transfer)
        {
            //simple, i've done it already
            transfer.Complete();
            return new ReadOnlyCollection<Transaction>(new List<Transaction>());
        }

        private IReadOnlyCollection<Transaction> WhenSecondTransactionFaulted(Transfer transfer)
        {
            //the most difficult case... should create compensating transaction for the first account 
            //as first transaction was already completed and you should return money anyway..
            throw new NotImplementedException();
        } 
    }
}
