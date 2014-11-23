using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions.Policies.Card
{
    [Injectable]
    public class UserCardBlockedPolicy : DisallowPolicy
    {
        private readonly IRepository<UserCard> _userCardRepository;

        public UserCardBlockedPolicy(IRepository<UserCard> userCardRepository)
        {
            Argument.NotNull(userCardRepository, "userCardRepository");

            _userCardRepository = userCardRepository;
        }

        public override bool AppliesTo(Transaction transaction, BankOperation operation)
        {
            if (transaction.Type == TransactionType.Deposit)
            {
                return false;
            }
            var cardTransaction = transaction as CardTransaction;
            if (cardTransaction == null)
            {
                return false;
            }
            var userCard = _userCardRepository.Find(cardTransaction.Card.Id);
            return userCard != null && userCard.Settings.Blocked;
        }

        public override int Priority
        {
            get { return 2; }
        }

        public override string GetErrorMessage(Transaction transaction, BankOperation operation)
        {
            return Messages.CardBlocked;
        }
    }
}
