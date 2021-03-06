﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Transfers.Entities;
using VaBank.Data.Tests.EntityFramework.Mocks;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public class ProcessingSchemaTest: EntityFrameworkTest
    {        
        [TestCategory("Development")]
        public void Can_VaBank_Context_Save_OperationCategory()
        {
            var context = Context;

            var operatonCategory1 = new OperationCategoryMock("code1", "oc1", "operation category 1");
            var operatonCategory2 = new OperationCategoryMock("code2", "oc2", "operation category 2");
            var operatonCategory3 = new OperationCategoryMock("code3", "oc3", "operation category 3");

            operatonCategory2.Parent = operatonCategory1;
            operatonCategory3.Parent = operatonCategory1;

            context.Set<OperationCategory>().Add(operatonCategory1);
            context.Set<OperationCategory>().Add(operatonCategory2);
            context.Set<OperationCategory>().Add(operatonCategory3);
            context.SaveChanges();
        }

        public void Can_VaBank_Context_Save_Operation()
        {
            var context = Context;
            var operationCategory = context.Set<OperationCategory>().Single(x => x.Code == "oc1");
            context.SaveChanges();
        }

        [TestCategory("Development")]
        [TestMethod]        
        public void Can_Vabank_Context_Save_CardTransaction()
        {
            var currency = Context.Set<Currency>().Find("USD");
            var fromCard = Context.Set<UserCard>().First();
            var cardTransaction = new CardTransaction("ABC", "Test_Card_Transaction", "Belarus, Minsk, Karastayanova str. 43-18", fromCard.Account, fromCard, currency, 50, 50);
            Context.Set<CardTransaction>().Add(cardTransaction);
            Context.SaveChanges();
        }

        [TestCategory("Development")]
        [TestMethod]        
        public void Can_Vabank_Context_Save_CardTransfer()
        {          
            var operationCategory = Context.Set<OperationCategory>().Find("TRANSFER-CARD");
            var cards = Context.Set<UserCard>().OrderBy(x => x.CardNo)
                .Where(x => x.Account != null)
                .Take(2)
                .ToList();
            var fromCard = cards[0];
            var toCard = cards[1];
            var transfer = new CardTransfer(operationCategory, fromCard, toCard, 50);
            Context.Set<CardTransfer>().Add(transfer);
            Context.SaveChanges();
        }
    }
}
