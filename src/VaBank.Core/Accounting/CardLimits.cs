using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting
{
    public class CardLimits: Entity
    {
        public int OperationsPerDayLocal { get; set; }
        public int OperationsPerDayAbroad { get; set; }
        public decimal AmountPerDayLocal { get; set; }
        public decimal AmountPerDayAbroad { get; set; }
    }
}
