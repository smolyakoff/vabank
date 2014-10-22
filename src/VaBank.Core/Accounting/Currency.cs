using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting
{
    public class Currency: Entity
    {
        public Currency(string isoName, string symbol, string fullName)
        {
            if (String.IsNullOrWhiteSpace(isoName))
            {
                throw new ArgumentException("Invalid currency ISO name");
            }
        }

        public string ISOName { get; set; }
        public string Symbol { get; set; }
        public string FullName { get; set; }
    }
}
