using VaBank.Common.Data.Database;

namespace VaBank.Jobs.Common
{
    public interface ITransactionalJobContext
    {
        ITransactionFactory TransactionFactory { get; }
    }
}
