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
            public static LinqSpec<CardTransaction> ForToday(TimeZoneInfo timeZone)
            {
                var now = DateTime.UtcNow;
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(now, timeZone);
                var startOfDay = localTime.Date;
                var endOfDay = localTime.Date.AddDays(1);
                return LinqSpec.For<CardTransaction>(x => x.CreatedDateUtc >= startOfDay && x.CreatedDateUtc < endOfDay);
            }

            public static LinqSpec<CardTransaction> NotFailed = LinqSpec.For<CardTransaction>(x => x.Status != ProcessStatus.Failed);
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
