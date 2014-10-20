using System;

namespace VaBank.Services.Contracts.Common.Security
{
    public class AccessDeniedException : SecurityException
    {
        public AccessDeniedException(AccessDenied reason) : base(reason.UserMessage)
        {
            if (reason == null)
            {
                throw new ArgumentNullException("reason");
            }
            Reason = reason;
        }

        public AccessDenied Reason { get; private set; }
    }
}
