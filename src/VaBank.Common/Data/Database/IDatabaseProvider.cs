using System.Data.Common;

namespace VaBank.Common.Data.Database
{
    public interface IDatabaseProvider : IConnectionProvider, ITransactionProvider
    {
        DbDataAdapter CreateAdapter();

        DbCommand CreateCommand();

        DbParameter CreateParameter();
    }
}
