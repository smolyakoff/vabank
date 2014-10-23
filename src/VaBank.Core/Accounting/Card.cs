using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting
{
    public class Card: Entity<Guid>
    {
        public Card(Guid userId, Guid cardId)
        {
            if (cardId == Guid.Empty)
            {
                throw new ArgumentException("Invalid card id");
            }
            Id = cardId;
        }

        public Guid UserId { get; set; }

        public string CardNo { get; set; }

        public CardVendor CardVendor { get; set; }

        public string CardholderFirstName { get; set; }

        public string CardholderLastName { get; set; }

        public string SecureCardNo { get; set; }

        public DateTime ExpirationDateUtc { get; set; }
    }
}
