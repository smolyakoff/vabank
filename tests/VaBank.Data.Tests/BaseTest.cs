using Autofac;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using VaBank.Common.Data.Database;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
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
                .AsSelf()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof (Repository<>).Assembly)
                .Where(t => typeof (IRepository).IsAssignableFrom(t) && !t.IsGenericType)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof (Repository<>))
                .As(typeof (IRepository<>))
                .As(typeof (IQueryRepository<>))
                .As(typeof (IPartialQueryRepository<>))
                .InstancePerLifetimeScope();

            var injectables = typeof(Entity).Assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<InjectableAttribute>() != null)
                .Select(t => new { Lifetime = t.GetCustomAttribute<InjectableAttribute>().Lifetime, Type = t })
                .ToList();

            foreach (var injectable in injectables)
            {
                var registration = builder.RegisterType(injectable.Type).AsImplementedInterfaces().AsSelf();
                switch (injectable.Lifetime)
                {
                    case Lifetime.Singleton:
                        registration.SingleInstance();
                        break;
                    case Lifetime.PerDependency:
                        registration.InstancePerDependency();
                        break;
                }
            }

            Container = builder.Build();
        }
    }
}
