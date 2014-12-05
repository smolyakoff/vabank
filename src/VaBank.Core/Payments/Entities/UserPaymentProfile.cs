using System;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class UserPaymentProfile : Entity
    {
        public UserPaymentProfile(User user, string fullName, string payerTIN, string address)
        {
            Argument.NotNull(user, "user");
            Argument.NotEmpty(fullName, "fullName");
            Argument.NotEmpty(payerTIN, "payerTIN");
            Argument.NotEmpty(address, "address")

            User = user;
            UserId = user.Id;
            FullName = fullName;
            PayerTIN = payerTIN;
            Address = address;
        }

        public Guid UserId { get; private set; }

        public virtual User User { get; protected set; }

        public string FullName { get; protected set; }

        public string PayerTIN { get; protected set; }

        public string Address { get; protected set; }
    }
}
