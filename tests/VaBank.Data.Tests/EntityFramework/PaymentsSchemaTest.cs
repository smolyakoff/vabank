using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Payments.Entities;
using VaBank.Data.Tests.EntityFramework.Mocks;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public class PaymentsSchemaTest : EntityFrameworkTest
    {
        [TestCategory("Development")]
        //[TestMethod]
        public void Can_VaBank_Context_Save_PaymentOrder_And_PaymentTransaction()
        {
            var priorBank = Context.Set<Bank>().Single(x => x.Code == "153001749");
            var priorCorrAcc = Context.Set<CorrespondentAccount>().Single(x => x.AccountNo == "3014153001749");
            var user = Context.Set<User>().Single(x => x.UserName == "terminator");
            long paymentNumber = 123;

            var paymentOrder = new PaymentOrder(
                paymentNumber,
                user.Profile.FirstName + " " + user.Profile.LastName,
                "153001966",
                "3014077960602",
                "MA1953684",
                "Velcom",
                priorBank.Code,
                "3012202410089",
                "101528843",
                String.Format("Пополнение счета. Номер телефона: {0}", user.Profile.PhoneNumber),
                100000,
                "BYR",
                "1234");

            var cardAccount = Context.Set<CardAccount>().Single(x => x.Owner.Id == user.Id);
            var card = cardAccount.Cards.First();
            var currency = Context.Set<Currency>().Single(x => x.ISOName == "BYR");

            var paymentTransaction = new CardPaymentTransaction(
                paymentOrder,
                cardAccount.Cards.First(),
                card.Account,
                currency,
                10000,
                card.Account.Balance,
                "PAYMENT-CELL-VELCOM-PHONENO",
                "Пополнение счета. Номер телефона",
                "location");

            Context.Set<PaymentOrder>().Add(paymentOrder);
            Context.Set<CardPaymentTransaction>().Add(paymentTransaction);
            Context.SaveChanges();


        }

        [TestCategory("Development")]
        //[TestMethod]
        public void Can_VaBank_Context_Save_Payment_And_CardPayment()
        {
            var priorBank = Context.Set<Bank>().Single(x => x.Code == "153001749");
            var priorCorrAcc = Context.Set<CorrespondentAccount>().Single(x => x.AccountNo == "3014153001749");
            long paymentNumber = 123;
            var user = Context.Set<User>().Single(x => x.UserName == "terminator");
            var cardAccount = Context.Set<CardAccount>().Single(x => x.Owner.Id == user.Id);
            var card = cardAccount.Cards.First();
            var paymentOrder = new PaymentOrder(
                paymentNumber,
                user.Profile.FirstName + " " + user.Profile.LastName,
                "153001966",
                "3014077960602",
                "MA1953684",
                "Velcom",
                priorBank.Code,
                "3012202410089",
                "101528843",
                String.Format("Пополнение счета. Номер телефона: {0}", user.Profile.PhoneNumber),
                100000,
                "BYR",
                "1234");
            var paymentTemplate = Context.Set<PaymentTemplate>().Single(x => x.Code == "PAYMENT-CELL-VELCOM-PHONENO");
            var currency = Context.Set<Currency>().Single(x => x.ISOName == "BYR");
            var cardPayment = new CardPayment(
                card,
                paymentOrder,
                paymentTemplate,
                "Арнольд Шварценегерр",
                paymentTemplate.Code,
                paymentTemplate,
                card.Account,
                priorCorrAcc,
                currency,
                100000);
            Context.Set<CardPayment>().Add(cardPayment);
            Context.SaveChanges();
        }

        [TestCategory("Development")]
        [TestMethod]
        public void Can_VaBank_Context_Get_PaymentTemplates_And_PaymentOrderTemplates()
        {
            var paymentTemplates = Context.Set<PaymentTemplate>().Include(x => x.OrderTemplate).ToList();

            Assert.IsTrue(paymentTemplates.Count > 0);
        }

        [TestCategory("Development")]
        [TestMethod]
        public void Can_VaBank_Context_Save_UserPaymentProfile()
        {
            var user = Context.Set<User>().Single(x => x.UserName == "terminator");
            var userPaymentProfile = new UserPaymentProfile(user, "Арнольд Шварценегерр", "123456789", "Калифорния");
            Context.Set<UserPaymentProfile>().Add(userPaymentProfile);
            Context.SaveChanges();
        }
    }
}
