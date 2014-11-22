using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Processing.Entities
{
    public class CardTransaction : Transaction
    {
        internal CardTransaction(
            string code,
            string description,
            string location,
            Account account,
            Card card, 
            Currency transactionCurrency, 
            decimal transactionAmount,
            decimal accountAmount) 
            : base(account, transactionCurrency, transactionAmount, accountAmount, code, description, location)
        {
            Argument.NotNull(card, "card");
            Card = card;
        }


        public virtual Card Card { get; protected set; }
    }
}
