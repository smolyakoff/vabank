using System;

namespace VaBank.Common.Data.Repositories
{
    public class ConcurrencyException : RepositoryException
    {
        private const string DefaultMessage = "Concurrent data access error occured. Please refresh the data.";

        public ConcurrencyException(Exception innerException) : base(DefaultMessage, innerException)
        {
        }

        public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(message);
            }
        }
    }
}
