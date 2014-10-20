namespace VaBank.Services.Contracts.Membership.Commands
{
    public class CreateUserCommand
    {
        public string UserName { get; set; }

        public string Role { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

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
