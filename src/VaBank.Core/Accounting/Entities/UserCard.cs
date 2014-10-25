using VaBank.Common.Validation;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Accounting.Entities
{
    public class UserCard: Card
    {
        public UserCard(Card card, User user, CardSettings settings) 
        {
            Argument.NotNull(user, "user");
            Argument.NotNull(card, "card");
            Argument.NotNull(settings, "settings");
            Id = card.Id;
            CardNo = card.CardNo;
            HolderFirstName = card.HolderFirstName;
            HolderLastName = card.HolderLastName;
            ExpirationDateUtc = card.ExpirationDateUtc;
            CardVendor = card.CardVendor;
            User = user;
            Settings = settings;
        }

        public UserCard(CardAccount cardAccount, Card card, User user, CardSettings settings)
            :this(card, user, settings)
        {
            Argument.NotNull(cardAccount, "cardAccount");
            Account = cardAccount;
        } 

        public virtual User User { get; private set; }

        public virtual CardSettings Settings { get; private set; }

        public virtual CardAccount Account { get; internal set; }
    }
}
