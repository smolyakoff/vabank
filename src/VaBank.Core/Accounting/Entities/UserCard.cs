using System;
using VaBank.Common.Validation;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Accounting.Entities
{
    public class UserCard : Card
    {
        public UserCard(Card card, User owner, CardSettings settings) 
        {
            Argument.NotNull(owner, "user");
            Argument.NotNull(card, "card");
            Argument.NotNull(settings, "settings");
            Id = card.Id;
            CardNo = card.CardNo;
            HolderFirstName = card.HolderFirstName;
            HolderLastName = card.HolderLastName;
            ExpirationDateUtc = card.ExpirationDateUtc;
            CardVendor = card.CardVendor;
            Owner = owner;
            Settings = settings;
        }

        public UserCard(CardAccount account, Card card, User owner, CardSettings settings)
            :this(card, owner, settings)
        {
            Argument.NotNull(account, "cardAccount");
            Account = account;
        }

        protected UserCard()
        {
        }

        public virtual User Owner { get; private set; }

        public virtual CardSettings Settings { get; private set; }

        public virtual CardAccount Account { get; private set; }

        public void LinkTo(CardAccount account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }
            if (Account == null)
            {
                Account = account;
                account.Cards.Add(this);
                return;
            }
            if (account.AccountNo == Account.AccountNo)
            {
                return;
            }
            Account.Cards.Remove(this);
            Account = account;
            account.Cards.Add(this);
        }

        public void Unlink()
        {
            Account.Cards.Remove(this);
            Account = null;
        }

        public void Block()
        {
            Settings.Blocked = true;
            Settings.BlockedDateUtc = DateTime.UtcNow;
        }

        public void Unblock()
        {
            Settings.Blocked = false;
            Settings.BlockedDateUtc = null;
        }
    }
}
