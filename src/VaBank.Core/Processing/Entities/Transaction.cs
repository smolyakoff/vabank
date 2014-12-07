using System;
using System.Security.Cryptography.X509Certificates;
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
            string code,
            string description,
            string location) : this()
        {
            Argument.NotNull(account, "account");
            Argument.NotNull(transactionCurrency, "transactionCurrency");
            Argument.NotNull(code, "code");
            Argument.NotEmpty(description, "description");

            Account = account;
            Currency = transactionCurrency;
            TransactionAmount = transactionAmount;
            AccountAmount = accountAmount;
            Code = code;
            Description = description;
            Location = location;

            if (Type == TransactionType.Withdrawal)
            {
                Account.Withdraw(-AccountAmount);
            }
            RemainingBalance = account.Balance;
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

        public TransactionType Type
        {
            get { return TransactionAmount > 0 ? TransactionType.Deposit : TransactionType.Withdrawal; }
        }

        public DateTime CreatedDateUtc { get; protected set; }

        public DateTime? PostDateUtc { get; set; }

        public string Code { get; protected set; }

        public string Description { get; protected set; }

        public string Location { get; protected set; }

        public string ErrorMessage { get; protected set; }

        public ProcessStatus Status { get; protected set; }

        public virtual void Fail(string message)
        {
            Argument.NotNull(message, "message");
            if (Status != ProcessStatus.Pending)
            {
                throw new DomainException("Current state is already final.");
            }

            if (Type == TransactionType.Withdrawal)
            {
                Account.Deposit(-AccountAmount);
            }
            RemainingBalance = Account.Balance;
            ErrorMessage = message;
            Status = ProcessStatus.Failed;
        }

        public virtual void Complete(DateTime postDateUtc)
        {
            Argument.Satisfies(postDateUtc, x => x >= DateTime.UtcNow.Date, "postDateUtc", "Post date should be today or later.");
            if (Status != ProcessStatus.Pending)
            {
                throw new DomainException("Current state is already final.");
            }

            if (Type == TransactionType.Deposit)
            {
                Account.Deposit(AccountAmount);
            }
            RemainingBalance = Account.Balance;
            PostDateUtc = postDateUtc;
            Status = ProcessStatus.Completed;
        }
    }
}
