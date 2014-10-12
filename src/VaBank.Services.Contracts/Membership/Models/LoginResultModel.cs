using System;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Membership.Models
{
    public abstract class LoginResultModel
    {
        private readonly UserMessage _message;

        protected LoginResultModel(UserMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            _message = message;
        }


        public abstract bool IsSuccessful { get; }

        public UserMessage Message
        {
            get { return _message; }
        }
    }
}
