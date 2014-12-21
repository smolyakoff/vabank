using System;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Core.Processing.Exceptions;

namespace VaBank.Core.Accounting.Entities
{
    public abstract class Account : Entity, IVersionedEntity
    {
        protected Account(Currency currency, string type)
            :this()
        {
            Argument.NotNull(currency, "currency");
            Argument.NotNull(type, "type");
            Currency = currency;
            Type = type;
        }

        protected Account()
        {
            OpenDateUtc = DateTime.UtcNow;
        }

        public string AccountNo { get; protected set; }

        public virtual Currency Currency { get; set; }

        public decimal Balance { get; protected set; }

        public DateTime OpenDateUtc { get; protected set; }

        public DateTime ExpirationDateUtc { get; internal set; }

        public string Type { get; protected set; }

        public byte[] RowVersion { get; protected set; }

        public bool IsExpired
        {
            get { return ExpirationDateUtc.Date <= DateTime.UtcNow; }
        }

        internal virtual Account Deposit(decimal amount)
        {
            Argument.Satisfies(amount, x => x >= 0, "amount", "Deposit amount should be zero or greater.");

            Balance += amount;
            return this;
        }

        internal virtual Account Withdraw(decimal amount)
        {
            Argument.Satisfies(amount, x => x >= 0, "amount", "Withdrawal amount should be zero or greater.");
            //By default, we do not allow credits.
            if (amount > Balance)
            {
                throw new InsufficientFundsException(Balance, amount);
            }
            Balance -= amount;
            return this;
        }
    }
}
