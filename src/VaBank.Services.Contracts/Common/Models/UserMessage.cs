using System;

namespace VaBank.Services.Contracts.Common.Models
{
    public class UserMessage
    {
        private readonly string _message;

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
            _message = message;
            Code = code;
        }

        public string Message { get; private set; }

        public int? Code { get; set; }
    }
}
