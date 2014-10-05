using System.Collections.Generic;

namespace VaBank.Services.Contracts.Common.Validation
{
    public class ValidationResponse
    {
        public ValidationResponse()
        {
            ValidationFaults = new List<ValidationFault>();
        }

        public bool IsValidatorFound { get; set; }

        public IList<ValidationFault> ValidationFaults { get; set; }

        public bool IsValid
        {
            get { return ValidationFaults.Count == 0; }
        }
    }
}
