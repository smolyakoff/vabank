using System;
using System.Data.Common;

namespace VaBank.Common.Data.Database
{
    public interface ITransactionProvider
    {
        DbTransaction CurrentTransaction { get; }

        bool HasCurrentTransaction { get; }

        //TODO: remove this hack. Now i know how to create transaction before db context is instantiated
        event EventHandler TransactionStarted;
    }
}
