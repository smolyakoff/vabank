using VaBank.Common.Validation;

namespace VaBank.Core.Accounting.Entities
{
    public class CardAccount: UserAccount
    {
        public CardAccount(string accountNo, Currency currency, UserCard card)
            :base(currency, card.User)
        {
            Argument.NotNull(card, "card");
            AccountNo = accountNo;
            Type = "CardAccount";
            card.Account = this;
            Card = card;
        }

        protected CardAccount()
        {
            Type = "CardAccount";
        }

        public virtual UserCard Card { get; protected set; }

        public override string Type { get; protected set; }
    }
}
