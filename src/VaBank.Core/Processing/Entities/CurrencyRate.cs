using System;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Common;

namespace VaBank.Core.Processing.Entities
{
    public class CurrencyRate : Entity<Guid>
    {
        protected CurrencyRate() { }

        public Currency From { get; protected set; }

        public Currency To { get; protected set; }

        public decimal BuyRate { get; protected set; }

        public decimal SellRate { get; protected set; }

        public DateTime TimestampUtc { get; protected set; }

        public bool IsActual { get; protected set; }

        public static CurrencyRate Create(Currency from, Currency to, 
            decimal buyingRate, decimal sellingRate, DateTime timeStampUtc, bool isActual)
        {
            if (from == null)
                throw new ArgumentNullException("from");
            if (to == null)
                throw new ArgumentNullException("to");
            if (from.ISOName == to.ISOName)
                throw new InvalidOperationException("Can't create rate to same currency type.");

            if (string.CompareOrdinal(from.ISOName, to.ISOName) > 1)
            {
                var temp = to;
                to = from;
                from = temp;
            }
            return new CurrencyRate
            {
                BuyRate = buyingRate,
                SellRate = sellingRate,
                From = from,
                To = to,
                TimestampUtc = timeStampUtc,
                Id = Guid.NewGuid(),
                IsActual = isActual
            };
        }
    }
}
