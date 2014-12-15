using System;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Payments;

namespace VaBank.Core.Accounting.Entities
{
    public class UserPaymentProfile : Entity
    {
        public UserPaymentProfile(User user, string fullName, string payerTIN, string address)
        {
            Argument.NotNull(user, "user");
            Argument.NotEmpty(fullName, "fullName");
            Argument.EnsureIsValid<TINValidator, string>(payerTIN, "payerTIN");
            Argument.NotEmpty(address, "address");

            User = user;
            UserId = user.Id;
            FullName = fullName;
            PayerTIN = payerTIN;
            Address = address;
        }

        protected UserPaymentProfile()
        {
        }

        public Guid UserId { get; private set; }

        public virtual User User { get; protected set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string PayerTIN { get; protected set; }

        public string PayerName
        {
            get { return string.Format("{0}, {1}", FullName, Address); }
        }
    }
}
