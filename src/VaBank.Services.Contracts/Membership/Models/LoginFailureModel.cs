using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Membership.Models
{
    public class LoginFailureModel : LoginResultModel
    {
        private readonly LoginFailureReason _reason;

        public LoginFailureModel(UserMessage message, LoginFailureReason reason) : base(message)
        {
            _reason = reason;
        }

        public LoginFailureReason Reason
        {
            get { return _reason; }
        }

        public override bool IsSuccessful
        {
            get { return false; }
        }
    }
}
