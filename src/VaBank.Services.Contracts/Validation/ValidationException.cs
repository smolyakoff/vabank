using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Services.Contracts.Validation
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException(string message, IList<ValidationFault> faults)
        {
            Message = message;
            Faults = faults;
        }

        public string Message { get; private set; }
        public IList<ValidationFault> Faults { get; private set; }
    }
}
