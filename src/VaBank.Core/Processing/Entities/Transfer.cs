using System;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Processing.Entities
{
    public abstract class Transfer : BankOperation
    {
        private Transaction _deposit;

        protected Transfer(OperationCategory category, Account from, Account to, Currency currency, decimal amount) 
            : this(category, currency, amount)
        {
            Argument.NotNull(from, "from");
            Argument.NotNull(to, "to");
            From = from;
            To = to;     
        }

        protected Transfer(OperationCategory category, Currency currency, decimal amount) : base(category)
        {
            Argument.NotNull(currency, "currency");
            Argument.Satisfies(amount, x => x > 0, "amount", "Amount should be greater than zero.");
            Currency = currency;
            Amount = amount;
        }

        protected Transfer()
        {
        }

        public virtual Account From { get; protected set; }

        public virtual Account To { get; protected set; }

        public virtual Transaction Withdrawal { get; internal set; }

        public virtual Transaction Deposit
        {
            get { return _deposit; }
            set
            {
                if (value == null)
                {
                    _deposit = null;
                    return;
                }
                if (value.AccountNo != To.AccountNo)
                {
                    throw new ArgumentException("Deposit transaction should target destination account.");
                }
                _deposit = value;
            }
        }

        public virtual Currency Currency { get; protected set; }

        public decimal Amount { get; protected set; }

        public Money MoneyAmount
        {
            get {return new Money(Currency, Amount);}
        }
    }
}
