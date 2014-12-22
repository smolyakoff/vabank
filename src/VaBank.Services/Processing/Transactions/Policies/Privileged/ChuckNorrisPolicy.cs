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
        private readonly MoneyConverter _converter;

        private readonly IRepository<UserBankOperation> _userBankOperationRepository; 

        public ChuckNorrisPolicy(MoneyConverter converter, IRepository<UserBankOperation> userBankOperationRepository)
        {
            Argument.NotNull(converter, "converter");
            Argument.NotNull(userBankOperationRepository, "userBankOperationRepository");
            _converter = converter;
            _userBankOperationRepository = userBankOperationRepository;
        }

        public override bool AppliesTo(Transaction transaction, BankOperation operation)
        {
            var userAccount = transaction.Account as UserAccount;
            if (userAccount == null)
            {
                return false;
            }
            if (operation != null)
            {
                var userOperation = _userBankOperationRepository.Find(operation.Id);
                if (userOperation != null && userOperation.User.UserName == "chuck")
                {
                    return false;
                }
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
