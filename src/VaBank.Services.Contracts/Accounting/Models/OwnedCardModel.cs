using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class OwnedCardModel : CardModel
    {
        public UserNameModel Owner { get; set; }

        public string AccountNo { get; set; }
    }
}
