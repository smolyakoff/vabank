using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Payments.Entities;
using VaBank.Core.Processing.Entities;
using VaBank.Data.Tests.EntityFramework.Mocks;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public class PaymentsSchemaTest : EntityFrameworkTest
    {
        [TestCategory("Development")]
        [TestMethod]
        public void Can_VaBank_Context_Save_PaymentOrder_And_CardPayment()
        {
            long paymentNumber = 123;
            var priorCorrAcc = Context.Set<CorrespondentAccount>().Single(x => x.AccountNo == "3014153001749");
            var user = Context.Set<User>().Single(x => x.UserName == "terminator");
            var userPaymentProfile = Context.Set<UserPaymentProfile>().Single(x => x.UserId == user.Id);
            var userAccount = Context.Set<UserAccount>().First(x => x.Owner.Id == user.Id);
            var cardAccount = Context.Set<CardAccount>().Single(x => x.Owner.Id == user.Id && x.AccountNo == userAccount.AccountNo);
            var paymentOrderTemplate = Context.Set<PaymentOrderTemplate>().Single(x => x.TemplateCode == "PAYMENT-CELL-VELCOM-PHONENO");
            var currency = Context.Set<Currency>().Single(x => x.ISOName == "BYR");
            var paymentTemplate = Context.Set<PaymentTemplate>().Single(x => x.Code == paymentOrderTemplate.TemplateCode);
            var card = cardAccount.Cards.First();

            var paymentOrder = new PaymentOrder(
                paymentNumber,
                user.Profile.FirstName + " " + user.Profile.LastName,
                "153001966",
                cardAccount.AccountNo,
                userPaymentProfile.PayerTIN,
                paymentOrderTemplate.BeneficiaryName,
                paymentOrderTemplate.BeneficiaryBankCode,
                paymentOrderTemplate.BeneficiaryAccountNo,
                paymentOrderTemplate.BeneficiaryTIN,
                String.Format("Пополнение счета. Номер телефона: {0}", user.Profile.PhoneNumber),
                100000,
                currency.ISOName,
                paymentOrderTemplate.PaymentCode);

            var assembly = Assembly.GetExecutingAssembly();
            var form = JObject.Parse(new StreamReader(assembly.GetManifestResourceStream("VaBank.Data.Tests.EntityFramework.Templates.cell-velcom-phoneno.json")).ReadToEnd());
            var accountTo = Context.Set<CorrespondentAccount>().Single(x => x.Bank.Code == paymentOrderTemplate.BeneficiaryBankCode);
            var cardPayment = new CardPayment(card, paymentTemplate, paymentOrder, form, cardAccount, accountTo, currency);

            var paymentTransaction = new Transaction(cardAccount, currency, 100000, cardAccount.Balance, "123", "test_transaction_desc", "Nezavisimosti 58");
            var paymentTransactionLink = new PaymentTransactionLink(paymentTransaction, paymentOrder);

            Context.Set<PaymentOrder>().Add(paymentOrder);
            Context.Set<CardPayment>().Add(cardPayment);
            Context.SaveChanges();


        }

        [TestCategory("Development")]
        //[TestMethod]
        public void Can_VaBank_Context_Get_PaymentTemplates_And_PaymentOrderTemplates()
        {
            var paymentTemplates = Context.Set<PaymentTemplate>().Include(x => x.OrderTemplate).ToList();

            Assert.IsTrue(paymentTemplates.Count > 0);
        }
    }
}
