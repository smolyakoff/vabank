using System;
using System.Linq;
using Autofac;
using AutoMapper;
using FluentValidation;
using VaBank.Services;
using VaBank.Services.Contracts;
using VaBank.Services.Validation;
using VaBank.UI.Web.Validation;

namespace VaBank.UI.Web.Modules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var mappingProfiles =
                typeof (BaseService).Assembly.GetTypes().Where(t => typeof (Profile).IsAssignableFrom(t)).ToList();
            mappingProfiles.ForEach(x => Mapper.AddProfile(Activator.CreateInstance(x) as Profile));

            //Register validation system
            builder.RegisterType<DefaultValidationFactory>().As<IValidationFactory>().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof (IValidationFactory).Assembly)
                .AsClosedTypesOf(typeof(AbstractValidator<>)).InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof (BaseService).Assembly)
                .Where(t => typeof (IService).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .InstancePerRequest();
        }
    }
}