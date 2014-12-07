using System;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CardNameModel
    {
        public Guid CardId { get; set; }

        public string SecureCardNo { get; set; }

        public string FriendlyName { get; set; }
    }
}
