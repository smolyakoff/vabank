using System;

namespace VaBank.Services.Contracts.Membership.Models
{
    public class ProtectedTicketModel
    {
        public ProtectedTicketModel(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            Value = value;
        }

        public string Value { get; private set; }
    }
}
