using System;
using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting.Entities
{
    public class Card : Entity<Guid>
    {
        public Card(
            string cardNo, 
            CardVendor cardVendor,
            string holderFirstName,
            string holderLastName,
            DateTime expirationDateUtc)
        {
            Argument.NotNull(cardVendor, "cardVendor");
            Argument.NotEmpty(holderFirstName, "holderFirstName");
            Argument.NotEmpty(holderLastName, "holderLastName");
            Id = Guid.NewGuid();
            CardNo = cardNo;
            CardVendor = cardVendor;
            HolderFirstName = holderFirstName;
            HolderLastName = holderLastName;
            ExpirationDateUtc = expirationDateUtc;
        }

        internal Card()
        {
        }

        public string CardNo { get; protected set; }

        public string HolderFirstName { get; protected set; }

        public string HolderLastName { get; protected set; }

        public DateTime ExpirationDateUtc { get; protected set; }

        public virtual CardVendor CardVendor { get; protected set; }
    }
}
