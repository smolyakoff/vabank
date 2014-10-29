
using System;
using System.Linq;

namespace VaBank.Data.Migrations
{
    internal class M2Card
    {
        public static M2Card Create(string accountNo, Guid userId, string userName)
        {
            return new M2Card
            {
                CardId = Guid.NewGuid(),
                AccountNo = accountNo,
                UserId = userId,
                CardVendorId = "visa",
                ExpirationDateUtc = DateTime.Today.AddDays(360),
                HolderFirstName = userName.ToUpper(),
                HolderLastName = new string(userName.Reverse().ToArray()).ToUpper(),
                CardNo = "4666" + Seed.RandomStringOfNumbers(2) + "00" + Seed.RandomStringOfNumbers(8)
            };
        }

        public Guid CardId { get; private set; }

        public string CardNo { get; private set; }

        public string AccountNo { get; private set; }

        public Guid UserId { get; private set; }

        public string CardVendorId { get; private set; }

        public string HolderFirstName { get; private set; }

        public string HolderLastName { get; private set; }

        public DateTime ExpirationDateUtc { get; set; }

        public object ToCard()
        {
            return new {CardId, CardNo, CardVendorId, HolderFirstName, HolderLastName, ExpirationDateUtc};
        }

        public object ToUserCard()
        {
            return new {UserId, CardId, AccountNo};
        }
    }
}
