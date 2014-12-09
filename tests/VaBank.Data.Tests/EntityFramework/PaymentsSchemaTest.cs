using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Payments.Entities;
using VaBank.Data.Tests.EntityFramework.Mocks;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public class PaymentsSchemaTest: EntityFrameworkTest
    {
        [TestCategory("Development")]
        [TestMethod]

        public void Can_VaBank_Context_Save_Payments_Schema_Tables()
        {
            var priorBank = Context.Set<Bank>().Single(x => x.Code == "153001749");
            var priorCorrAcc = Context.Set<CorrespondentAccount>().Single(x => x.AccountNo == "3014153001749");
            var user = Context.Set<User>().Single(x => x.UserName == "terminator");
            //var payerAccountNo = "3014077960602";
            //var payerName = "Арнольд Шварцнеггер";
            //var 
            long paymentNumber = 123;

            var paymentOrder = new PaymentOrderMock(
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
                "PAYMENT-CELL-VELCOM-PHONENO");

            Context.Set<PaymentOrder>().Add(paymentOrder);
            Context.SaveChanges();

            //    "153001966",
            //    "3014077960602",
            //    "payerTIN",
            //    "Velcom",
            //    "153001749",
            //    "beneficiaryAccountNo",
            //    "101528843",
            //    "Мобильная связь",
            //     100000,
            //    "BYR",
            //    "paymentCode"
            //);


        }
    }
}
