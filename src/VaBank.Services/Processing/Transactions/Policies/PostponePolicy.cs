using System;
using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions.Policies
{
    public abstract class PostponePolicy : IPolicy
    {
        public abstract DateTime GetScheduledDateUtc(Transaction transaction, BankOperation operation);

        public abstract bool AppliesTo(Transaction transaction, BankOperation operation);

        public abstract int Priority { get; }
    }
}
