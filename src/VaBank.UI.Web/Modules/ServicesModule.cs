using Autofac;
using AutoMapper;
using FluentValidation;
using System;
using System.Linq;
using VaBank.Services.Common;
using VaBank.Services.Contracts;
using VaBank.UI.Web.Api.Infrastructure.Validation;
using IValidatorFactory = VaBank.Services.Contracts.Common.Validation.IValidatorFactory;
using Module = Autofac.Module;

namespace VaBank.UI.Web.Modules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Add auto mapper profiles
            var mappingProfiles =
                typeof (BaseService).Assembly.GetTypes().Where(t => typeof (Profile).IsAssignableFrom(t)).ToList();
            mappingProfiles.ForEach(x => Mapper.AddProfile(Activator.CreateInstance(x) as Profile));

            //Register validation system
            builder.RegisterType<AutofacValidatorFactory>().As<IValidatorFactory>().InstancePerRequest();
            var validatorTypes = typeof (BaseService).Assembly.GetTypes()
                .Where(t => typeof (IValidator).IsAssignableFrom(t))
                .Where(t => !t.IsGenericType)
                .ToList();
            validatorTypes.ForEach(t => builder.RegisterType(t).AsImplementedInterfaces().InstancePerRequest());

            //Register repository collections
            builder.RegisterAssemblyTypes(typeof (BaseService).Assembly)
                .Where(t => typeof (IRepositoryCollection).IsAssignableFrom(t))
                .PropertiesAutowired()
                .AsSelf()
                .InstancePerRequest();

            //Register services
            builder.RegisterAssemblyTypes(typeof (BaseService).Assembly)
                .Where(t => typeof (IService).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .InstancePerRequest();
        }
    }
}