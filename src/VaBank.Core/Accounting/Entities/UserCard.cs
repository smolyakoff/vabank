using System;
using VaBank.Common.Data.Linq;
using VaBank.Common.Validation;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Accounting.Entities
{
    public class UserCard : Card
    {
        public static class Spec
        {
            public static LinqSpec<UserCard> Linked = LinqSpec.For<UserCard>(x => x.Account != null);

            public static LinqSpec<UserCard> ByCardNumberAndExpiration(string cardNumber, DateTime expirationDateUtc)
            {
                return Linked && LinqSpec.For<UserCard>(x => x.CardNo == cardNumber); // && 
                //x.ExpirationDateUtc.Month == expirationDateUtc.Month && 
                //x.ExpirationDateUtc.Year == expirationDateUtc.Year);
            } 
        }

        internal UserCard(Card card, User owner, CardSettings settings) 
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

        internal UserCard(CardAccount account, Card card, User owner, CardSettings settings)
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

        public bool IsExpired
        {
            get
            {
                var now = DateTime.UtcNow;
                return now.Year >= ExpirationDateUtc.Year && now.Month >= ExpirationDateUtc.Month;
            }
        }

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
            if (Account == null)
            {
                return;
            }
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
