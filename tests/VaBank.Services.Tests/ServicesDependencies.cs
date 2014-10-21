using Autofac;
using AutoMapper;
using FluentValidation;
using System;
using System.Data.Entity;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Database;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Events;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.App;
using VaBank.Core.Common;
using VaBank.Data.EntityFramework;
using VaBank.Data.EntityFramework.Common;
using VaBank.Services.Common;
using VaBank.Services.Contracts;

namespace VaBank.Data.Tests
{
    public static class ServicesDependencies
    {
        public static IContainer Container { get; private set; }
        
        static ServicesDependencies()
        {
            InitializeContainer();
        }

        public static IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();
            InitializeDataModule(builder);
            InitializeServicesModule(builder);
            Container = builder.Build();
            return Container;
        }

        private static void InitializeDataModule(ContainerBuilder builder)
        {
            //Initialize data module
            builder.RegisterType<ConfigurationFileDatabaseProvider>()
                .WithParameter("connectionStringName", "Vabank.Db")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<VaBankContext>()
                .As<DbContext>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
                .Where(t => typeof(IRepository).IsAssignableFrom(t) && !t.IsGenericType)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .As(typeof(IQueryRepository<>))
                .InstancePerLifetimeScope();
        }

        private static void InitializeServicesModule(ContainerBuilder builder)
        {
            //Add auto mapper profiles
            var mappingProfiles =
                typeof(BaseService).Assembly.GetTypes().Where(t => typeof(Profile).IsAssignableFrom(t)).ToList();
            mappingProfiles.ForEach(x => Mapper.AddProfile(Activator.CreateInstance(x) as Profile));

            //Register validation system
            builder.RegisterType<AutofacFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<JsonNetConverter>().AsImplementedInterfaces().InstancePerLifetimeScope();
            var validatorTypes = typeof(BaseService).Assembly.GetTypes()
                .Union(typeof(Entity).Assembly.GetTypes())
                .Where(t => typeof(IValidator).IsAssignableFrom(t) || typeof(IObjectValidator).IsAssignableFrom(t))
                .Where(t => !t.IsGenericType)
                .ToList();
            var staticValidators =
                validatorTypes.Where(t => t.IsDefined(typeof(StaticValidatorAttribute), false)).ToList();
            var otherValidators = validatorTypes.Except(staticValidators).ToList();

            staticValidators.ForEach(t => builder.RegisterType(t).AsImplementedInterfaces().AsSelf().SingleInstance());
            otherValidators.ForEach(t => builder.RegisterType(t).AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope());

            //Register operation provider
            builder.RegisterType<ServiceOperationProvider>().AsSelf()
                .Named<IOperationProvider>("Service")
                .InstancePerLifetimeScope();

            //Register dependency collections
            builder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
                .Where(t => typeof(IDependencyCollection).IsAssignableFrom(t))
                .PropertiesAutowired()
                .AsSelf()
                .InstancePerLifetimeScope();

            //Register service bus
            builder.RegisterInstance(new InMemoryServiceBus())
                .AsImplementedInterfaces()
                .SingleInstance();

            //Register services
            builder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
                .Where(t => typeof(IService).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
