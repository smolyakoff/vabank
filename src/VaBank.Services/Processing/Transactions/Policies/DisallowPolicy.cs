using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions.Policies
{
    public abstract class DisallowPolicy : IPolicy
    {
        public abstract bool AppliesTo(Transaction transaction, BankOperation operation);

        public abstract int Priority { get; }

        public abstract string GetErrorMessage(Transaction transaction, BankOperation operation);
    }
}
