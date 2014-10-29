using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class UserCardModel : CardModel
    {
        public UserNameModel Owner { get; set; }

        public string AccountNo { get; set; }
    }
}
