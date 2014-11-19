using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Processing.Entities;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public class ProcessingSchemaTest: EntityFrameworkTest
    {
        [TestCategory("Development")]
        [TestMethod]
        public void Can_VaBank_Context_Save_OperationCategory()
        {
            var context = Context;

            var operatonCategory1 = new OperationCategory("code1", "oc1", "operation category 1");
            var operatonCategory2 = new OperationCategory("code2", "oc2", "operation category 2");
            var operatonCategory3 = new OperationCategory("code3", "oc3", "operation category 3");

            operatonCategory2.ParentCategory = operatonCategory1;
            operatonCategory3.ParentCategory = operatonCategory1;

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
    }
}
