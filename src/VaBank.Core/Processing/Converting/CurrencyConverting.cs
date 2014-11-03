using System;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Processing.Converting
{
    public class CurrencyConverting
    {
        public CurrencyConverting(Currency from, Currency to)
        {
            if (from == null)
                throw new ArgumentNullException("from");
            if (to == null)
                throw new ArgumentNullException("to");
            From = from;
            To = to;
        }
        
        public Currency From { get; protected set; }
        public Currency To { get; protected set; }
        public decimal Amount { get; set; }
    }
}
