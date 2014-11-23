using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions.Policies.Card
{
    [Injectable]
    public class UserCardExpiredPolicy : DisallowPolicy
    {
        private readonly IRepository<UserCard> _userCardRepository;

        public UserCardExpiredPolicy(IRepository<UserCard> userCardRepository)
        {
            Argument.NotNull(userCardRepository, "userCardRepository");
            _userCardRepository = userCardRepository;
        }

        public override bool AppliesTo(Transaction transaction, BankOperation operation)
        {
            var cardTransaction = transaction as CardTransaction;
            if (cardTransaction == null)
            {
                return false;
            }
            var userCard = _userCardRepository.Find(cardTransaction.Card.Id);
            return userCard != null && userCard.IsExpired;
        }

        public override int Priority
        {
            get { return 1; }
        }

        public override string GetErrorMessage(Transaction transaction, BankOperation operation)
        {
            return Messages.CardExpired;
        }
    }
}
