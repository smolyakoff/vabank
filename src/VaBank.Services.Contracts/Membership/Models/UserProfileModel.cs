using System;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Membership.Models
{
    public class UserProfileModel : IUserModel
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool SmsNotificationEnabled { get; set; }

        public bool SmsConfirmationEnabled { get; set; }

        public string SecretPhrase { get; set; }
    }
}
