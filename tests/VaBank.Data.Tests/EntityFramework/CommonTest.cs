using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Core.Common;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public class CommonTest : BaseTest
    {
        [TestMethod]
        public void Can_Get_Db_Version()
        {
            var dbInformationRepository = Container.Resolve<IDbInformationRepository>();
            var version = dbInformationRepository.GetDbVersion();
        }
    }
}
