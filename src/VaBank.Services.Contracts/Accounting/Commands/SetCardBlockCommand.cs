using System;

namespace VaBank.Services.Contracts.Accounting.Commands
{
    public class SetCardBlockCommand
    {
        public Guid CardId { get; set; }

        public bool Blocked { get; set; }
    }
}
