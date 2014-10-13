using System.Collections.Generic;

namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public class SecurityRuleMatchResult
    {
        public SecurityRuleMatchResult(IList<SecurityRuleFault> faults)
        {
            Faults = faults;
        }

        public IList<SecurityRuleFault> Faults { get; private set; }

        public bool IsMatch { get { return Faults == null || Faults.Count > 0; } }
    }
}
