using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Membership.Entities;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public class AccountingSchemaTest : EntityFrameworkTest
    {
        [TestMethod]
        public void Can_Insert_New_CardAccount()
        {
            var user = Context.Set<User>().FirstOrDefault(x => x.UserName == "terminator");
            var vendor = Context.Set<CardVendor>().Find("visa");
            var currency = Context.Set<Currency>().Find("USD");

            var card = new Card("1232123413241234", vendor, "TEST", "TEST", DateTime.Today.AddDays(300));
            var userCard = new UserCard(card, user, new CardSettings(card.Id, new CardLimits()));

            Context.Set<UserCard>().Add(userCard);

            var account = new CardAccount("1234567890123", currency, userCard) {ExpirationDateUtc = DateTime.UtcNow.AddDays(300)};

            Context.Set<CardAccount>().Add(account);
            Context.SaveChanges();
        }
    }
}
