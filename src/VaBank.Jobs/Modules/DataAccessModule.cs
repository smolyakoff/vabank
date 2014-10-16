using System.Data.Entity;
using Autofac;
using VaBank.Common.Data.Database;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Common;
using VaBank.Data.EntityFramework;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Jobs.Modules
{
    internal class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Entity framework data access module registration

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
        }
    }
}