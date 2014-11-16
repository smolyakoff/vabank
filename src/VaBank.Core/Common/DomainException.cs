using System;
using System.Runtime.Serialization;
using VaBank.Common.Validation;

namespace VaBank.Core.Common
{
    [Serializable]
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
            Argument.NotEmpty(message, "message");
        }

        public DomainException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DomainException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
