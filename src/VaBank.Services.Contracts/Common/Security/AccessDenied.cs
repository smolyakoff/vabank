using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Common.Security
{
    public abstract class AccessDenied
    {
        public string Code
        {
            get { return UserMessage.Code; }
        }

        public abstract UserMessage UserMessage { get; }
    }
}
