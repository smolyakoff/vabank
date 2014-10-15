using System.Linq;
using Autofac;
using FluentValidation;
using VaBank.Common.Data;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.App;
using VaBank.Core.Common;
using VaBank.Services.Common;
using VaBank.Services.Contracts;

namespace VaBank.Jobs.Modules
{
    internal class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //No need for automapper as it is the same app domain
            //var mappingProfiles =
            //    typeof (BaseService).Assembly.GetTypes().Where(t => typeof (Profile).IsAssignableFrom(t)).ToList();
            //mappingProfiles.ForEach(x => Mapper.AddProfile(Activator.CreateInstance(x) as Profile));

            //Register validation system
            builder.RegisterType<AutofacFactory>().AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterType<JsonNetConverter>().AsImplementedInterfaces().InstancePerRequest();
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
                .Named<IOperationProvider>("Service")
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