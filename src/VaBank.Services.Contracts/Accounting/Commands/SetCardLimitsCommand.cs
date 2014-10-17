using System;
using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Contracts.Accounting.Commands
{
    public class SetCardLimitsCommand : CardLimitsModel
    {
        public Guid CardId { get; set; }
    }
}
