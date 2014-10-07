using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Membership
{
    public class LoginSuccessModel : LoginResultModel
    {
        public LoginSuccessModel(UserMessage message) : base(message)
        {
        }

        public override bool IsSuccessful
        {
            get { return true; }
        }
    }
}
