using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing.Processors.Abstract
{
    public interface ITransactionProcessor<in TTransaction, in TBankOperation>
        where TTransaction : Transaction
        where TBankOperation : BankOperation
    {
        TransactionProcessorResult Process(TTransaction transaction, TBankOperation operation = null);
    }
}
