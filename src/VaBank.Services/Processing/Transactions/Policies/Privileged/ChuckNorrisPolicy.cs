using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions.Policies.Privileged
{
    [Injectable]
    public class ChuckNorrisPolicy : DisallowPolicy
    {
        private readonly IRepository<UserAccount> _userAccounts;
        private readonly MoneyConverter _converter;

        public ChuckNorrisPolicy(IRepository<UserAccount> userAccounts, MoneyConverter converter)
        {
            Argument.NotNull(userAccounts, "userAccounts");
            Argument.NotNull(converter, "converter");
            _converter = converter;
            _userAccounts = userAccounts;
        }

        public override bool AppliesTo(Transaction transaction, BankOperation operation)
        {            
            if (_userAccounts.Find(transaction.AccountNo).Owner.UserName == "chuck")
            {
                var amount = transaction.TransactionAmount;
                if (transaction.Currency.ISOName != "USD")
                {
                    amount = _converter.Convert(new Money(transaction.Currency, amount), "USD").Amount;
                }
                if ((transaction.Type == TransactionType.Withdrawal && amount < -10)
                    || (transaction.Type == TransactionType.Deposit && amount < 100))
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
