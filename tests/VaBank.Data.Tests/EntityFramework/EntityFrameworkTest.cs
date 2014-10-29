using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Common.Data.Database;
using VaBank.Data.EntityFramework;

namespace VaBank.Data.Tests.EntityFramework
{
    [TestClass]
    public abstract class EntityFrameworkTest
    {
        protected VaBankContext Context;

        [TestInitialize]
        public virtual void BeforeEachTest()
        {
            var databaseProvider = new ConfigurationFileDatabaseProvider("Vabank.Db");
            Context = new VaBankContext(databaseProvider, databaseProvider);
        }

        [TestCleanup]
        public virtual void AfterEachTest()
        {
            Context.Dispose();
            Context = null;
        }
    }
}
