using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Core.App.Entities;
using VaBank.Data.EntityFramework.App;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public class AppSchemaTest : EntityFrameworkTest
    {
        [TestMethod]
        [TestCategory("Development")]
        public void Can_Insert_And_Find_File_Link()
        {
            var link = FileLink.Create(new Uri("file.mp3", UriKind.RelativeOrAbsolute), FileLinkLocation.WebServer);
            Context.Set<FileLink>().Add(link);

            Context.SaveChanges();

            var foundLink = Context.Set<FileLink>().Find(link.Id);
            Assert.IsNotNull(foundLink);

            Assert.AreEqual("file.mp3", foundLink.Uri);
            Assert.AreEqual(FileLinkLocation.WebServer.ToString(), foundLink.Location);
        }

        [TestMethod]
        [TestCategory("Development")]
        public void Can_Start_And_Finish_Operations()
        {
            var repo = new OperationRepository(Context);
            using (var transaction = Context.Database.BeginTransaction())
            {

                var op1 = repo.Start("TEST", null);

                var op2 = repo.GetCurrent();
                var op3 = repo.GetCurrent();

                var ops = new List<Operation>() {op1, op2, op3};
                Assert.AreEqual(1, ops.Select(x => x.Id).Distinct().Count());

                repo.Stop(op1);

                var op4 = repo.GetCurrent();
                Assert.IsNull(op4);
                transaction.Commit();
            }

            var op5 = repo.GetCurrent();
            Assert.IsNull(op5);
        }
    }
}
