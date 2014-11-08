using System;
using VaBank.Core.Common;

namespace VaBank.Core.Processing.Entities
{
    public class CurrencyRate : Entity
    {
        protected CurrencyRate() { }

        public string FromISOName { get; protected set; }

        public string ToISOName { get; protected set; }

        public decimal BuyingRate { get; protected set; }

        public decimal SellingRate { get; protected set; }

        public DateTime TimestampUtc { get; protected set; }

        public static CurrencyRate Create(string fromISOName, string toISOName, 
            decimal buyingRate, decimal sellingRate, DateTime timeStampUtc)
        {
            if (string.IsNullOrEmpty(fromISOName))
                throw new ArgumentNullException("fromISOName");
            if (string.IsNullOrEmpty(toISOName))
                throw new ArgumentNullException("toISOName");

            if (string.CompareOrdinal(fromISOName, toISOName) > 1)
            {
                return new CurrencyRate
                {
                    BuyingRate = buyingRate,
                    SellingRate = sellingRate,
                    FromISOName = toISOName,
                    ToISOName = fromISOName,
                    TimestampUtc = timeStampUtc
                };
            }
            return new CurrencyRate
            {
                BuyingRate = buyingRate,
                SellingRate = sellingRate,
                FromISOName = fromISOName,
                ToISOName = toISOName,
                TimestampUtc = timeStampUtc
            };
        }
    }
}
