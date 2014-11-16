using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Processing.Entities
{
    public class CardTransaction : Transaction
    {
        internal CardTransaction(
            string description,
            string location,
            UserCard card, 
            Currency transactionCurrency, 
            decimal transactionAmount,
            decimal accountAmount,
            decimal remainingBalance)
        {
            Argument.NotEmpty(description, "description");
            Argument.NotNull(card, "card");
            Argument.Satisfies(card.Account, x => x != null, "card", "Card should be bound to a bank account.");
            Description = description;
            Location = location;
            Account = card.Account;
            Card = card;
            Currency = transactionCurrency;
            TransactionAmount = transactionAmount;
            AccountAmount = accountAmount;
            RemainingBalance = remainingBalance;
        }

        public virtual Card Card { get; protected set; }
    }
}
