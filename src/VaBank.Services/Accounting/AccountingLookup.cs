using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Accounting;
using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Accounting
{
    internal class AccountingLookup : AccountingLookupModel
    {
        internal AccountingLookup(IEnumerable<Currency> currencies)
        {
        }
    }
}
