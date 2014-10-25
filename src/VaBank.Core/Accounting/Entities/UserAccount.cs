using VaBank.Common.Validation;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Accounting.Entities
{
    public abstract class UserAccount : Account
    {
        protected UserAccount(Currency currency, User user)
            :base(currency)
        {
            Argument.NotNull(user, "user");
            User = user;
        }

        protected UserAccount()
        {
        }

        public virtual User User { get; protected set; }
    }
}
