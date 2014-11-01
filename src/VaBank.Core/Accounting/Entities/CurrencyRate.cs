using System;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting.Entities
{
    public class CurrencyRate : Entity
    {
        protected CurrencyRate() { }

        public string BuyingCurrencyISOName { get; protected set; }

        public string SellingCurrencyISOName { get; protected set; }

        public decimal Rate { get; set; }

        public DateTime Date { get; protected set; }

        public static CurrencyRate Create(string buyingCurrencyISOName, string sellingCurrencyISOName, 
            decimal rate, DateTime date)
        {
            if (string.IsNullOrEmpty(buyingCurrencyISOName))
                throw new ArgumentNullException("buyingCurrencyISOName");
            if (string.IsNullOrEmpty(sellingCurrencyISOName))
                throw new ArgumentNullException("sellingCurrencyISOName");
            return new CurrencyRate
            {
                BuyingCurrencyISOName = buyingCurrencyISOName,
                SellingCurrencyISOName = sellingCurrencyISOName,
                Date = date,
                Rate = rate
            };
        }
    }
}
