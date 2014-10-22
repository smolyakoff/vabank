using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting
{
    public class CardVendor: Entity<string>
    {
        public CardVendor(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Invalid card vendor id");
            }
            else
            {
                Id = id;
            }
        }
    }
}
