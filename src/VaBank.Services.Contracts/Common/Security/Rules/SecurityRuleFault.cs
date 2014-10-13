using System;

namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public class SecurityRuleFault
    {
        public SecurityRuleFault(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException("message");
            Message = message;
        }

        public string Message { get; private set; }
    }
}
