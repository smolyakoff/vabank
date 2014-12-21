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
            public static LinqSpec<UserCard> Active = LinqSpec.For<UserCard>(x => x.IsActive);

            public static LinqSpec<UserCard> ByCardNumberAndExpiration(string cardNumber, DateTime expirationDateUtc)
            {
                return Active && LinqSpec.For<UserCard>(x => x.CardNo == cardNumber &&
                    x.ExpirationDateUtc.Month == expirationDateUtc.Month && 
                    x.ExpirationDateUtc.Year == expirationDateUtc.Year);
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

        public bool IsActive { get; set; }

        public bool IsExpired
        {
            get { return DateTime.UtcNow.Date >= ExpirationDateUtc.Date; }
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
