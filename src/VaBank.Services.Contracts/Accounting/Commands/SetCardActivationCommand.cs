using System;

namespace VaBank.Services.Contracts.Accounting.Commands
{
    public class SetCardActivationCommand
    {
        public Guid CardId { get; set; }

        public bool IsActive { get; set; }
    }
}
