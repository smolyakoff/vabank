using VaBank.Common.IoC;
using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions.Policies.Account
{
    [Injectable]
    public class AccountExpiredPolicy : DisallowPolicy
    {
        public override bool AppliesTo(Transaction transaction, BankOperation operation)
        {
            return transaction.Account.IsExpired;
        }

        public override int Priority
        {
            get { return 1; }
        }

        public override string GetErrorMessage(Transaction transaction, BankOperation operation)
        {
            return Messages.AccountExpired;
        }
    }
}
