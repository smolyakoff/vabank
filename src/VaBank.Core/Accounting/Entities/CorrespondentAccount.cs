using VaBank.Common.Validation;

namespace VaBank.Core.Accounting.Entities
{
    public class CorrespondentAccount : Account
    {
        internal CorrespondentAccount(Currency currency, Bank bank)
            : base(currency)
        {
            Argument.NotNull(bank, "bank");
            Bank = bank;
        }

        protected CorrespondentAccount()
        {
        }

        public virtual Bank Bank { get; private set; }
    }
}
