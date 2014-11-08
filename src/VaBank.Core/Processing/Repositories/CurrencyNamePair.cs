using System;

namespace VaBank.Core.Processing.Repositories
{
    public class CurrencyNamePair
    {
        public CurrencyNamePair(string fromISOName, string toISOName)
        {
            if (string.IsNullOrEmpty(fromISOName))
                throw new ArgumentNullException("fromISOName");
            if (string.IsNullOrEmpty(toISOName))
                throw new ArgumentNullException("toISOName");

            FromISOName = fromISOName;
            ToISOName = toISOName;
        }

        public string FromISOName { get; protected set; }

        public string ToISOName { get; protected set; }
    }
}
