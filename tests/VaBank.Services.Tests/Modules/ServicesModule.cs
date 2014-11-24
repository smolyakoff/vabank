using System.Configuration;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using FluentValidation;
using VaBank.Common.Data;
using VaBank.Common.IoC;
using VaBank.Common.Util;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Services.Common;
using VaBank.Services.Common.Transactions;
using VaBank.Services.Contracts;

namespace VaBank.Services.Tests.Modules
{
    internal class ServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Register injectables
            LoadInjectables(builder);
            LoadSettings(builder);

            //Register automapper profiles
            builder.RegisterAssemblyTypes(typeof (BaseService).Assembly)
                .Where(t => typeof (Profile).IsAssignableFrom(t) && !t.IsAbstract)
                .As<Profile>()
                .SingleInstance();

            //Register validation system
            builder.RegisterType<AutofacFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<JsonNetConverter>().AsImplementedInterfaces().SingleInstance();
            var validatorTypes = typeof (BaseService).Assembly.GetTypes()
                .Union(typeof(Entity).Assembly.GetTypes())
                .Where(t => typeof (IValidator).IsAssignableFrom(t) || typeof(IObjectValidator).IsAssignableFrom(t))
                .Where(t => !t.IsGenericType)
                .ToList();
            var staticValidators =
                validatorTypes.Where(t => t.IsDefined(typeof (StaticValidatorAttribute), false)).ToList();
            var otherValidators = validatorTypes.Except(staticValidators).ToList();

            staticValidators.ForEach(t => builder.RegisterType(t).AsImplementedInterfaces().AsSelf().SingleInstance());
            otherValidators.ForEach(t => builder.RegisterType(t).AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope());

            //Register identity
            builder.RegisterType<VaBankIdentity>()
                .AsSelf()
                .InstancePerLifetimeScope();

            //Register operation provider
            builder.RegisterType<ServiceOperationProvider>().AsSelf()
                .InstancePerLifetimeScope();

            //Register dependency collections
            builder.RegisterAssemblyTypes(typeof (BaseService).Assembly)
                .Where(t => typeof (IDependencyCollection).IsAssignableFrom(t))
                .PropertiesAutowired()
                .AsSelf()
                .InstancePerLifetimeScope();

            //Register services
            builder.RegisterAssemblyTypes(typeof (BaseService).Assembly)
                .Where(t => typeof (IService).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        private static void LoadInjectables(ContainerBuilder builder)
        {
            var injectables = typeof(BaseService).Assembly.GetTypes()
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
        }

        private static void LoadSettings(ContainerBuilder builder)
        {
            var settings = typeof(BaseService).Assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<SettingsAttribute>() != null)
                .ToList();
            foreach (var setting in settings)
            {
                var copied = setting;
                builder.Register(c => c.Resolve<SettingsManager>().Load(copied))
                    .As(copied)
                    .InstancePerLifetimeScope();
            }
        }
    }
}