using VaBank.Common.Validation;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Accounting.Entities
{
    public abstract class UserAccount : Account
    {
        protected UserAccount(Currency currency, User owner, Bank bank)
            :base(currency, bank)
        {
            Argument.NotNull(owner, "user");
            Owner = owner;
        }

        protected UserAccount()
        {
        }

        public virtual User Owner { get; protected set; }
    }
}
