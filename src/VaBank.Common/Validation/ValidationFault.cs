using System;

namespace VaBank.Common.Validation
{
    public class ValidationFault
    {
        public ValidationFault(string propertyName, string message)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }
            PropertyName = propertyName;
            Message = message;
        }

        public string PropertyName { get; private set; }

        public string Message { get; private set; }
    }
}