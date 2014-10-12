using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Membership.Models
{
    public class LoginFailureModel : LoginResultModel
    {
        private readonly LoginFailure _reason;

        public LoginFailureModel(UserMessage message, LoginFailure reason) : base(message)
        {
            _reason = reason;
        }

        public LoginFailure Reason
        {
            get { return _reason; }
        }

        public override bool IsSuccessful
        {
            get { return false; }
        }
    }
}
