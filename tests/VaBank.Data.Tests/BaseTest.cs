using Autofac;
using System.Data.Entity;
using VaBank.Common.Data.Database;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Common;
using VaBank.Data.EntityFramework;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.Tests
{
    public abstract class BaseTest
    {
        protected IContainer Container;

        public BaseTest()
        {
            InitializeContainer();
        }

        protected virtual void InitializeContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ConfigurationFileDatabaseProvider>()
                .WithParameter("connectionStringName", "Vabank.Db")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<VaBankContext>()
                .As<DbContext>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof (Repository<>).Assembly)
                .Where(t => typeof (IRepository).IsAssignableFrom(t) && !t.IsGenericType)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof (Repository<>))
                .As(typeof (IRepository<>))
                .As(typeof (IQueryRepository<>))
                .InstancePerLifetimeScope();

            Container = builder.Build();
        }
    }
}
