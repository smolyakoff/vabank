using System;
using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting.Entities
{
    public abstract class Account : Entity
    {
        protected Account(Currency currency)
            :this()
        {
            Argument.NotNull(currency, "currency");
            Currency = currency;
        }

        protected Account()
        {
            OpenDateUtc = DateTime.UtcNow;
        }

        public string AccountNo { get; protected set; }

        public virtual Currency Currency { get; set; }

        public decimal Balance { get; set; }

        public DateTime OpenDateUtc { get; protected set; }

        public DateTime ExpirationDateUtc { get; set; }

        public string Type { get; protected set; }
    }
}
