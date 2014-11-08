using System;
using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting.Entities
{
    public abstract class Account : Entity, IVersionedEntity
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

        public DateTime ExpirationDateUtc { get; internal set; }

        public string Type { get; protected set; }

        public byte[] RowVersion { get; protected set; }
    }
}
