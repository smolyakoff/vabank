using System;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Membership.Models
{
    public class LoginSuccessModel : LoginResultModel
    {
        public LoginSuccessModel(UserMessage message, UserIdentityModel user) : base(message)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            User = user;
        }

        public UserIdentityModel User { get; private set; }

        public override bool IsSuccessful
        {
            get { return true; }
        }
    }
}
