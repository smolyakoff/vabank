using System;
using System.Runtime.Serialization;

namespace VaBank.Services.Contracts.Common
{
    [Serializable]
    public class ServiceException : Exception
    {
        private const string DefaultMessage = "Unexpected error occured.";

        public ServiceException() : base(DefaultMessage)
        {
        }

        public ServiceException(string message) : base(message)
        {
        }

        public ServiceException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ServiceException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
