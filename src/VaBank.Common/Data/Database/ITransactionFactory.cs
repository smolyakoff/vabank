using System.Data;
using System.Data.Common;

namespace VaBank.Common.Data.Database
{
    public interface ITransactionFactory
    {
        DbTransaction BeginTransaction(IsolationLevel isolationLevel);
    }
}
