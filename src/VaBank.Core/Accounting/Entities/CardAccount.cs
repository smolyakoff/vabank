using System.Collections.Generic;
using System.Collections.ObjectModel;
using VaBank.Common.Validation;

namespace VaBank.Core.Accounting.Entities
{
    public class CardAccount : UserAccount
    {
        public CardAccount(string accountNo, Currency currency, UserCard card)
            :base(currency, card.Owner)
        {
            Argument.NotNull(card, "card");
            AccountNo = accountNo;
            Type = "CardAccount";
            Cards = new Collection<UserCard>();
            card.LinkTo(this);
        }

        protected CardAccount()
        {
            Type = "CardAccount";
        }

        public virtual ICollection<UserCard> Cards { get; protected set; }
    }
}
