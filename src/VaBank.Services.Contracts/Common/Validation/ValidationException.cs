using System;
using System.Collections.Generic;

namespace VaBank.Services.Contracts.Common.Validation
{
    [Serializable]
    public class ValidationException : ServiceException
    {
        private const string DefaultMessage = "Some validation errors occured.";

        public ValidationException()
            : this(DefaultMessage, new List<ValidationFault>())
        {
        }

        public ValidationException(string message) : this(message, new List<ValidationFault>())
        {
        }

        public ValidationException(string message, IList<ValidationFault> faults) : base(message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }
            if (faults == null)
            {
                throw new ArgumentNullException("faults");
            }
            Faults = faults;
        }

        public IList<ValidationFault> Faults { get; private set; }
    }
}
