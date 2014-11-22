using System;
using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions.Policies
{
    public abstract class CompletePolicy : IPolicy
    {
        public abstract bool AppliesTo(Transaction transaction, BankOperation operation);
        public abstract int Priority { get; }
        public abstract DateTime GetPostDateUtc(Transaction transaction, BankOperation operation);
    }
}
