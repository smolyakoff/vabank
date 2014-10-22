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
        public CardVendor CardVendor { get; set; }

        public string CardNo { get; set; }

        public string CardholderFirstName { get; set; }

        public string CardholderLastName { get; set; }

        public DateTime CardExpirationDateUtc { get; set; }

        public DateTime AccountOpenedDateUtc { get; set; }

        public DateTime AccountExpirationDateUtc
        {
            get { return CardExpirationDateUtc; }
        }

        public CardLimits CardLimits { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
