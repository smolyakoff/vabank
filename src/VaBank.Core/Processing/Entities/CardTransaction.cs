using System;
using VaBank.Common.Data.Linq;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Processing.Entities
{
    public class CardTransaction : Transaction
    {
        public static class Spec
        {
            public static LinqSpec<CardTransaction> ForToday(Guid cardId, TimeZoneInfo timeZone)
            {
                var now = DateTime.UtcNow;
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(now, timeZone);
                var startOfDay = localTime.Date;
                var endOfDay = localTime.Date.AddDays(1);
                return LinqSpec.For<CardTransaction>(x => x.Card.Id == cardId && x.CreatedDateUtc >= startOfDay && x.CreatedDateUtc < endOfDay);
            }

            public static LinqSpec<CardTransaction> Failed = LinqSpec.For<CardTransaction>(x => x.Status == ProcessStatus.Failed);

            public static LinqSpec<CardTransaction> Completed = LinqSpec.For<CardTransaction>(x => x.Status == ProcessStatus.Completed);

            public static LinqSpec<CardTransaction> Withdrawals = LinqSpec.For<CardTransaction>(x => x.TransactionAmount < 0);

            public static LinqSpec<CardTransaction> Deposits = LinqSpec.For<CardTransaction>(x => x.TransactionAmount > 0);

            public static LinqSpec<CardTransaction> CalculatedWithdrawals = Withdrawals && !Failed;

            public static LinqSpec<CardTransaction> CalculatedDeposits = Deposits && Completed;
        }

        internal CardTransaction(
            string code,
            string description,
            string location,
            Account account,
            Card card, 
            Currency transactionCurrency, 
            decimal transactionAmount,
            decimal accountAmount) 
            : base(account, transactionCurrency, transactionAmount, accountAmount, code, description, location)
        {
            Argument.NotNull(card, "card");
            Card = card;
        }

        protected CardTransaction()
        {
        }

        public virtual Card Card { get; protected set; }
    }
}
