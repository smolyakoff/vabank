using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Threading;
using Autofac;
using Autofac.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Common.Data;
using VaBank.Common.Data.Database;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Membership.Entities;
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

        protected User AuthenticateTerminator()
        {
            var users = Scope.Resolve<IQueryRepository<User>>();
            var user = users.QueryOne(DbQuery.For<User>().FilterBy(x => x.UserName == "terminator"));
            var identity = new ClaimsIdentity("Test");
            foreach (var userClaim in user.Claims)
            {
                identity.AddClaim(new Claim(userClaim.Type, userClaim.Value));
            }
            identity.AddClaim(new Claim(UserClaim.Types.UserId, user.Id.ToString()));
            Thread.CurrentPrincipal = new ClaimsPrincipal(identity);
            return user;
        }
    }
}
