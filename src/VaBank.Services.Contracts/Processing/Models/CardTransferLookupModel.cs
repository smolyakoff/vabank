using System.Collections.Generic;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class CardTransferLookupModel
    {
        public Dictionary<string, decimal> MinimalAmountsByCurrency { get; set; } 
    }
}
