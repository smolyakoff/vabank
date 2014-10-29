using System;

namespace VaBank.Core.Accounting.Exceptions
{
    public class CardLimitsNotFoundException : Exception
    {
        public CardLimitsNotFoundException(string currencyISOName, string message = null)
        {
            if (string.IsNullOrEmpty(currencyISOName))
                throw new ArgumentNullException("currencyISOName");
            CurrencyISOName = currencyISOName;
            Message = message;
        }

        public string Message { get; private set; }

        public string CurrencyISOName { get; private set; }
    }
}
