using System;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Common
{
    public class UserMessageException : ServiceException
    {
        public UserMessageException(UserMessage message) : base(message.Message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            UserMessage = message;
        }

        public UserMessage UserMessage { get; private set; }
    }
}
