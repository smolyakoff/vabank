using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions
{
    public interface ITransactionProcessor : IProcessor<TransactionProcessorCommand>
    {
        bool CanProcess(Transaction transaction, BankOperation operation);
    }
}
