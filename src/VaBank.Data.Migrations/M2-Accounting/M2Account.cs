using FluentMigrator;
using System;

namespace VaBank.Data.Migrations
{
    internal class M2Account
    {
        public string AccountNo { get; set; }

        public string CurrencyISOName { get; set; }

        public decimal Balance { get; set; }

        public DateTime OpenDateUtc { get; set; }

        public DateTime ExpirationDateUtc { get; set; }

        public ExplicitUnicodeString Type { get; set; }

        public static M2Account Create(string accountNo, string currencyISOName, decimal balance,
            DateTime openDateUtc, DateTime expirationDateUtc, string type)
        {
            return new M2Account
            {
                AccountNo = accountNo,
                CurrencyISOName = currencyISOName,
                Balance = balance,
                OpenDateUtc = openDateUtc,
                ExpirationDateUtc = expirationDateUtc,
                Type = new ExplicitUnicodeString(type)
            };
        }

        public M2UserAccount ToUserAccount(Guid userId)
        {
            return new M2UserAccount
            {
                AccountNo = this.AccountNo,
                UserId = userId
            };
        }
    }
}
