using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting
{
    public class Account: Entity
    {
        public Account(string accountNo)
        {
            AccountNo = accountNo;
        }

        public string AccountNo { get; set; }
        public Currency Currency { get; set; }
        public decimal Balance { get; set; }
        public DateTime OpenDateUtc { get; set; }
        public DateTime ExpirationDateUtc { get; set; }
        public string Type { get; set; }
    }
}
