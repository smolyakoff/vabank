using Autofac;
using AutoMapper;
using FluentValidation;
using System;
using System.Linq;
using System.Reflection;
using VaBank.Common.Data;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Jobs.Common;
using VaBank.Services.Common;
using VaBank.Services.Contracts;

namespace VaBank.Jobs.Modules
{
    internal class ServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {            
            //Register automapper profiles
            builder.RegisterAssemblyTypes(typeof (BaseJob<>).Assembly)
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