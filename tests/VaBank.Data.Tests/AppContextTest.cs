using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Core.App;

namespace VaBank.Data.Tests
{
    [TestClass]
    public class AppContextTest : EntityFrameworkTest
    {
        [TestMethod]
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

    }
}
