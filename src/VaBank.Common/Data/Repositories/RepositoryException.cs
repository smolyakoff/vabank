using System;
using System.Runtime.Serialization;

namespace VaBank.Common.Data.Repositories
{
    [Serializable]
    public class RepositoryException : Exception
    {
        private const string DefaultMessage = "Data repository error occured.";

        public RepositoryException() : base(DefaultMessage)
        {
        }

        public RepositoryException(string message) : base(message)
        {
        }

        public RepositoryException(string message, Exception inner) : base(message, inner)
        {
        }

        protected RepositoryException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
