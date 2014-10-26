using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Membership.Entities;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public class AccountingSchemaTest : EntityFrameworkTest
    {
        [TestMethod]
        [TestCategory("Development")]
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

        [TestMethod]
        [TestCategory("Development")]
        public void Can_Get_CardAccounts()
        {
            var accounts = Context.Set<CardAccount>().ToList();

            Assert.IsTrue(accounts.Count > 0);
        }

        [TestMethod]
        [TestCategory("Development")]
        public void Can_Insert_MultipleCards_With_Account()
        {
            var user = Context.Set<User>().FirstOrDefault(x => x.UserName == "terminator");
            var vendor = Context.Set<CardVendor>().Find("visa");
            var currency = Context.Set<Currency>().Find("USD");

            var card1 = new Card("2232123413241234", vendor, "TEST", "TEST", DateTime.Today.AddDays(300));
            var userCard1 = new UserCard(card1, user, new CardSettings(card1.Id, new CardLimits()));
            var card2 = new Card("3232123413241234", vendor, "TEST", "TEST", DateTime.Today.AddDays(300));
            var userCard2 = new UserCard(card2, user, new CardSettings(card2.Id, new CardLimits()));

            Context.Set<UserCard>().Add(userCard1);
            Context.Set<UserCard>().Add(userCard2);

            var account1 = new CardAccount("test", currency, userCard1) { ExpirationDateUtc = DateTime.UtcNow.AddDays(300) };
            account1.Cards.Add(userCard2);
            Context.Set<CardAccount>().Add(account1);

            Context.SaveChanges();
        }
    }
}
