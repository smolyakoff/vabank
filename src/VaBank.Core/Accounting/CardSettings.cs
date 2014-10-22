using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Core.Accounting
{
    public class CardSettings
    {
        public bool Blocked { get; set; }
        public DateTime BlockedDateUtc { get; set; }
        public string FriendlyName { get; set; }
        public CardLimits Limits { get; set; }
    }
}
