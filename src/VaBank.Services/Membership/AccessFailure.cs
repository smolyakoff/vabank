using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Common.Security;

namespace VaBank.Services.Membership
{
    internal class AccessFailure : AccessDenied
    {
        private readonly AccessFailureReason _failure;

        private readonly UserMessage _message;

        public static AccessDeniedException ExceptionBecause(AccessFailureReason reason)
        {
            return new AccessDeniedException(new AccessFailure(reason));
        }

        public AccessFailure(AccessFailureReason failure)
        {
            _failure = failure;
            _message = failure.ToUserMessage();
        }

        public override UserMessage UserMessage
        {
            get { return _message; }
        }
    }
}
