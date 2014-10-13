using System;
using System.Collections.Generic;

namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public class SecurityRuleException : ServiceException
    {
        public IList<SecurityRuleFault> Faults { get; private set; }               

        public SecurityRuleException(string message, IList<SecurityRuleFault> faults)
            : base(message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException("message");
            if (faults == null)
                throw new ArgumentNullException("faults");
            Faults = faults;
        }
    }
}
