using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions.Policies.Privileged
{
    [Injectable]
    public class ChuckNorrisPolicy : DisallowPolicy
    {
        private readonly IRepository<UserAccount> _userAccounts;

        public ChuckNorrisPolicy(IRepository<UserAccount> userAccounts)
        {
            Argument.NotNull(userAccounts, "userAccounts");

            _userAccounts = userAccounts;
        }

        public override bool AppliesTo(Transaction transaction, BankOperation operation)
        {            
            if (_userAccounts.Find(transaction.AccountNo).Owner.UserName == "chuck" && transaction.Currency.ISOName == "USD")
            {
                if ((transaction.Type == TransactionType.Withdrawal && transaction.TransactionAmount < -10)
                    || (transaction.Type == TransactionType.Deposit && transaction.TransactionAmount < 100))
                    return true;
            }

            return false;
        }

        public override int Priority
        {
            get { return int.MaxValue/2; }
        }

        public override string GetErrorMessage(Transaction transaction, BankOperation operation)
        {
            return Messages.ChuckNorrisPolicy;
        }
    }
}
