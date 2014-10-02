using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VaBank.Services.Contracts.Validation
{
    public class ValidationFault
    {
        public ValidationFault(string propertyName, string message)
        {
            PropertyName = propertyName;
            Message = message;
        }

        public string PropertyName { get; private set; }
        public string Message { get; private set; }
    }
}