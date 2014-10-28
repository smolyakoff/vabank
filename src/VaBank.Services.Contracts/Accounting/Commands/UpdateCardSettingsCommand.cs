using System;
using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Contracts.Accounting.Commands
{
    public class UpdateCardSettingsCommand
    {
        public Guid CardId { get; set; }

        public string FriendlyName { get; set; }

        public CardLimitsModel CardLimits { get; set; }
    }
}
