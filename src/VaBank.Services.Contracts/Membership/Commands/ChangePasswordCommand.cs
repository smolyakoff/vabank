namespace VaBank.Services.Contracts.Membership.Commands
{
    public class ChangePasswordCommand
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordConfirmation { get; set; }

    }
}
