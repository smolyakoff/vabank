using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting
{
    public class CardSettings: Entity<Guid>
    {
        public bool Blocked { get; set; }
        public DateTime BlockedDateUtc { get; set; }
        public string FriendlyName { get; set; }
        public int LimitOperationsPerDayLocal { get; set; }
        public int LimitOperationsPerDayAbroad { get; set; }
        public decimal LimitAmountPerDayLocal { get; set; }
        public decimal LimitAmountPerDayAbroad { get; set; }
    }
}
