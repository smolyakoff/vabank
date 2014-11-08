using System.Reflection;
using Autofac;
using AutoMapper;
using FluentValidation;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Jobs.Common;
using VaBank.Services.Common;
using VaBank.Services.Common.Transactions;
using VaBank.Services.Contracts;

namespace VaBank.Jobs.Modules
{
    internal class ServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Register injectables
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
    }
}