using System;
using System.Data.Common;

namespace VaBank.Common.Data.Database
{
    public interface ITransactionProvider
    {
        DbTransaction CurrentTransaction { get; }

        bool HasCurrentTransaction { get; }

        event EventHandler TransactionStarted;
    }
}
