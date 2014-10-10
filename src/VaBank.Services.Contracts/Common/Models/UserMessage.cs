using System;

namespace VaBank.Services.Contracts.Common.Models
{
    //TODO: consider to redisign this class to include string code (instead of int?) and localized message
    public class UserMessage
    {
        public static UserMessage Format(string format, params object[] parameters)
        {
            return new UserMessage(string.Format(format, parameters));
        }

        public static UserMessage Format(string format, int? code, params object[] parameters)
        {
            return new UserMessage(string.Format(format, parameters), code);
        }

        public UserMessage(string message, int? code = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }
            Message = message;
            Code = code;
        }

        public string Message { get; private set; }

        public int? Code { get; set; }
    }
}
