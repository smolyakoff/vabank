using System;
using VaBank.Services.Contracts.Common.Commands;

namespace VaBank.Services.Contracts.Accounting.Commands
{
    public class SetCardAssignmentCommand : IUserCommand
    {
        public Guid UserId { get; set; }

        public Guid CardId { get; set; }

        public bool Assigned { get; set; }
    }
}
