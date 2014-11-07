using System;

namespace VaBank.Services.Processing
{
    internal class NBRBCurrencyRate
    {
        public string ISOName { get; set; }

        public decimal Rate { get; set; }

        public DateTime Date { get; set; }
    }
}
