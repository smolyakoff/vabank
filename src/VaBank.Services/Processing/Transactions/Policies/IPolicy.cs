using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions.Policies
{
    public interface IPolicy
    {
        bool AppliesTo(Transaction transaction, BankOperation operation);

        int Priority { get; }
    }
}
