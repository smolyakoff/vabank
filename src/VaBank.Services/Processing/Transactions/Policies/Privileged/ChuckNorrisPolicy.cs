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
        private readonly MoneyConverter _converter;

        public ChuckNorrisPolicy(MoneyConverter converter)
        {
            Argument.NotNull(converter, "converter");
            _converter = converter;
        }

        public override bool AppliesTo(Transaction transaction, BankOperation operation)
        {
            var userAccount = transaction.Account as UserAccount;
            if (userAccount == null)
            {
                return false;
            }
            if (userAccount.Owner.UserName == "chuck")
            {
                var amount = transaction.TransactionAmount;
                if (transaction.Currency.ISOName != "USD")
                {
                    amount = _converter.Convert(new Money(transaction.Currency, amount), "USD").Amount;
                }
                if ((transaction.Type == TransactionType.Withdrawal && amount < -10)
                    || (transaction.Type == TransactionType.Deposit && amount < 100))
                {
                    return true;
                }
            }
            return false;
        }

        public override int Priority
        {
            //whatta fuck?
            get { return int.MaxValue/2; }
        }

        public override string GetErrorMessage(Transaction transaction, BankOperation operation)
        {
            return Messages.ChuckNorrisPolicy;
        }
    }
}
