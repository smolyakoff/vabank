using System;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class UserPaymentProfile : Entity
    {
        public UserPaymentProfile(User user, string fullName, string payerTIN)
        {
            Argument.NotNull(user, "user");
            Argument.NotEmpty(fullName, "fullName");
            Argument.NotEmpty(payerTIN, "payerTIN");
            
            User = user;
            UserId = user.Id;
            FullName = fullName;
            PayerTIN = payerTIN;
        }

        public Guid UserId { get; private set; }

        public virtual User User { get; protected set; }

        public string FullName { get; protected set; }

        public string PayerTIN { get; protected set; }
    }
}
