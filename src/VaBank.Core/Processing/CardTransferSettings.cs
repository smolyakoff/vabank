using System.Collections.Generic;
using VaBank.Common.Util;

namespace VaBank.Core.Processing
{
    [Settings("VaBank.Core.Processing.CardTransferSettings")]
    public class CardTransferSettings
    {
        public CardTransferSettings()
        {
            MinimalAmounts = new Dictionary<string, decimal>();
        }

        public Dictionary<string, decimal> MinimalAmounts { get; set; } 
    }
}
