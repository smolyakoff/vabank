using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Processing.Entities
{
    public class CardTransaction : Transaction
    {
        internal CardTransaction(
            UserCard card, 
            Currency transactionCurrency, 
            decimal transactionAmount,
            decimal accountAmount)
        {
            Argument.NotNull(card, "card");
            Argument.Satisfies(card.Account, x => x != null, "card", "Card should be bound to a bank account.");
            Account = card.Account;
            Card = card;
        }

        public virtual Card Card { get; protected set; }
    }
}
