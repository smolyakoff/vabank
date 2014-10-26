using System;

namespace VaBank.Services.Contracts.Accounting.Commands
{
    public class SetCardAssignmentCommand
    {
        public string AccountNo { get; set; }

        public Guid CardId { get; set; }

        public bool Assigned { get; set; }
    }
}
