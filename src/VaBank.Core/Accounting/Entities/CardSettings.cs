using System;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting.Entities
{
    public class CardSettings: Entity
    {
        public CardSettings(Guid cardId, CardLimits limits)
        {
            if (cardId == Guid.Empty)
            {
                throw new ArgumentException("Card id should not be empty", "cardId");
            }
            if (limits == null)
            {
                throw new ArgumentNullException("limits");
            }
            CardId = cardId;
            Limits = limits;
        }

        public Guid CardId { get; protected set; }

        public bool Blocked { get; private set; }

        public DateTime? BlockedDateUtc { get; private set; }

        public string FriendlyName { get; set; }

        public CardLimits Limits { get; private set; }

        public void Block()
        {
            Blocked = true;
            BlockedDateUtc = DateTime.UtcNow;
        }

        public void Unblock()
        {
            Blocked = false;
            BlockedDateUtc = null;
        }
    }
}
