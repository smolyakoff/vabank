using System;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Common;

namespace VaBank.Core.Processing.Entities
{
    public class Transaction : Entity<Guid>, ITransaction
    {
        private Account _account;

        internal Transaction(
            Account account,
            Currency transactionCurrency,
            decimal transactionAmount,
            decimal accountAmount,
            decimal remainingBalance,
            string description,
            string location) : this()
        {
            Argument.NotNull(account, "account");
            Argument.NotNull(transactionCurrency, "transactionCurrency");
            Argument.NotEmpty(description, "description");

            Account = account;
            Currency = transactionCurrency;
            TransactionAmount = transactionAmount;
            AccountAmount = accountAmount;
            RemainingBalance = remainingBalance;
            Description = description;
            Location = location;
        }

        protected Transaction()
        {
            Id = Guid.NewGuid();
            CreatedDateUtc = DateTime.UtcNow;
            Status = ProcessStatus.Pending;
        }

        public string AccountNo { get; private set; }

        public virtual Account Account
        {
            get { return _account; }
            protected set
            {
                _account = value;
                AccountNo = value == null ? null : value.AccountNo;
            }
        }

        public virtual Currency Currency { get; protected set; }

        public decimal TransactionAmount { get; protected set; }

        public decimal AccountAmount { get; protected set; }

        public decimal RemainingBalance { get; protected set; }

        public DateTime CreatedDateUtc { get; protected set; }

        public DateTime? PostDateUtc { get; set; }

        public string Description { get; protected set; }

        public string Location { get; protected set; }

        public string ErrorMessage { get; protected set; }

        public ProcessStatus Status { get; protected set; }

        public virtual void Fail(string message)
        {
            Argument.NotNull(message, "message");

            ErrorMessage = message;
            Status = ProcessStatus.Failed;
        }

        public virtual void Complete(DateTime postDateUtc)
        {
            Argument.EnsureIsValid<FutureDateValidator, DateTime>(postDateUtc, "postDateUtc");

            PostDateUtc = postDateUtc;
            Status = ProcessStatus.Completed;
        }
    }
}
