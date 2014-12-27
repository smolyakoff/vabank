using System;
using VaBank.Common.Data.Linq;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing
{
    public class Specs
    {
        public static class ForTransaction
        {
            public static LinqSpec<Transaction> Failed = LinqSpec.For<Transaction>(x => x.Status == ProcessStatus.Failed);

            public static LinqSpec<Transaction> Completed = LinqSpec.For<Transaction>(x => x.Status == ProcessStatus.Completed);

            public static LinqSpec<Transaction> Finished = Failed || Completed;

            public static LinqSpec<Transaction> Withdrawals = LinqSpec.For<Transaction>(x => x.TransactionAmount < 0);

            public static LinqSpec<Transaction> Deposits = LinqSpec.For<Transaction>(x => x.TransactionAmount > 0);

            public static LinqSpec<Transaction> CalculatedWithdrawals = Withdrawals && !Failed;

            public static LinqSpec<Transaction> CalculatedDeposits = Deposits && Completed;
        }

        public static class ForCardTransaction
        {
            public static LinqSpec<CardTransaction> Withdrawals = LinqSpec.For<CardTransaction>(x => x.TransactionAmount < 0);

            public static LinqSpec<CardTransaction> Failed = LinqSpec.For<CardTransaction>(x => x.Status == ProcessStatus.Failed);

            public static LinqSpec<CardTransaction> ForToday(Guid cardId, TimeZoneInfo timeZone)
            {
                var now = DateTime.UtcNow;
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(now, timeZone);
                var startOfDay = localTime.Date;
                var endOfDay = localTime.Date.AddDays(1);
                return LinqSpec.For<CardTransaction>(x => x.Card.Id == cardId && x.CreatedDateUtc >= startOfDay && x.CreatedDateUtc < endOfDay);
            }
        }
    }
}
