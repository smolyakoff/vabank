using System.Runtime.Remoting.Contexts;
using Autofac;
using Autofac.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Common.Data.Database;
using VaBank.Data.EntityFramework;
using VaBank.Services.Tests.Modules;

namespace VaBank.Services.Tests
{
    [TestClass]
    public class BaseTest
    {
        private static readonly ContainerBuilder Builder;

        private static IContainer Container;

        protected ILifetimeScope Scope;

        static BaseTest()
        {
            Builder = new ContainerBuilder();
            Builder.RegisterModule<ServiceTestModule>();
        }

        [AssemblyInitialize]
        public static void BeforeAllTests(TestContext context)
        {
            Container = Builder.Build();
        }

        [AssemblyCleanup]
        public static void AfterAllTests()
        {
            Container.Dispose();
        }

        [TestInitialize]
        public virtual void BeforeEachTest()
        {
            Scope = Container.BeginLifetimeScope();
        }

        [TestCleanup]
        public virtual void AfterEachTest()
        {
            Scope.Dispose();;
        }
    }
}
