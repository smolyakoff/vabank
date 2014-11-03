using System;

namespace VaBank.Core.Processing.Repositories
{
    public class CurrencyNamePair
    {
        public CurrencyNamePair(string buyingCurrencyISOName, string sellingCurrencyISOName)
        {
            if (string.IsNullOrEmpty(buyingCurrencyISOName))
                throw new ArgumentNullException("buyingCurrencyISOName");
            if (string.IsNullOrEmpty(sellingCurrencyISOName))
                throw new ArgumentNullException("sellingCurrencyISOName");

            BuyingCurrencyISOName = buyingCurrencyISOName;
            SellingCurrencyISOName = sellingCurrencyISOName;
        }

        public string BuyingCurrencyISOName { get; protected set; }

        public string SellingCurrencyISOName { get; protected set; }
    }
}
