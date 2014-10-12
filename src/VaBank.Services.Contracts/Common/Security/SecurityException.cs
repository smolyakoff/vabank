using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Common.Security
{
    public class SecurityException : UserMessageException
    {
        public SecurityException(UserMessage message) : base(message)
        {
        }

        public override bool TransactionRollback
        {
            get { return false; }
        }
    }
}
