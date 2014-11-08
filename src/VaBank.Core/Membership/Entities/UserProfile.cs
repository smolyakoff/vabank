using System;
using VaBank.Core.Common;

namespace VaBank.Core.Membership.Entities
{
    public class UserProfile : Entity, IVersionedEntity
    {
        public UserProfile(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Invalid user id");
            }
            UserId = userId;
        }

        protected UserProfile()
        {
        }

        public Guid UserId { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool SmsConfirmationEnabled { get; set; }
        public bool SmsNotificationEnabled { get; set; }
        public string SecretPhrase { get; set; }
        public byte[] RowVersion { get; set; }
    }
}