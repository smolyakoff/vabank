﻿using System;

namespace VaBank.Services.Contracts.Membership.Commands
{
    public class UpdateProfileCommand : IUserCommand
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool SmsNotificationEnabled { get; set; }

        public bool SmsConfirmationEnabled { get; set; }
    }
}