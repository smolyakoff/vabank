using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Core.Common;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public class CommonTest : BaseTest
    {
        [TestMethod]
        public void Can_Get_Server_Version()
        {
            var dbInformationRepository = Container.Resolve<IDbInformationRepository>();
            var server = dbInformationRepository.GetServerVersion();
            Assert.IsNotNull(server);
        }
    }
}
