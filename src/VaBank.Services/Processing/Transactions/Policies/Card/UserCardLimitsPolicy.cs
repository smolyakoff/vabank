using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions.Policies.Card
{
    [Injectable]
    public class UserCardLimitsPolicy : DisallowPolicy
    {
        private readonly IRepository<UserCard> _userCardRepository;

        private readonly IQueryRepository<CardTransaction> _cardTransactionRepository;

        private readonly BankingSystemSchedule _schedule;

        private readonly BankSettings _settings;

        public UserCardLimitsPolicy(IRepository<UserCard> userCardRepository,
            IQueryRepository<CardTransaction> cardTransactionRepository,
            BankingSystemSchedule schedule)
        {
            Argument.NotNull(userCardRepository, "userCardRepository");
            Argument.NotNull(cardTransactionRepository, "cardTransactionRepository");
            Argument.NotNull(schedule, "schedule");

            _userCardRepository = userCardRepository;
            _cardTransactionRepository = cardTransactionRepository;
            _schedule = schedule;
            _settings = new BankSettings();
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
            if (userCard == null)
            {
                return false;
            }
            var query = DbQuery.For<CardTransaction>()
                .FilterBy(CardTransaction.Spec.NotFailed && CardTransaction.Spec.ForToday(userCard.Id, _schedule.TimeZone));
            var transactionsForToday = _cardTransactionRepository.Query(query);
            var countForToday = transactionsForToday.Count;
            var amountForToday = transactionsForToday
                .Where(x => x.Type == TransactionType.Withdrawal)
                .Sum(x => -x.AccountAmount);
            var countLimit = _settings.IsLocalLocation(transaction.Location)
                ? userCard.Settings.Limits.OperationsPerDayLocal
                : userCard.Settings.Limits.OperationsPerDayAbroad;
            var amountLimit = _settings.IsLocalLocation(transaction.Location)
                ? userCard.Settings.Limits.AmountPerDayLocal
                : userCard.Settings.Limits.AmountPerDayAbroad;
            return countForToday > countLimit || amountForToday > amountLimit;
        }

        public override int Priority
        {
            get { return 3; }
        }

        public override string GetErrorMessage(Transaction transaction, BankOperation operation)
        {
            return Messages.CardLimitsExceeded;
        }
    }
}
