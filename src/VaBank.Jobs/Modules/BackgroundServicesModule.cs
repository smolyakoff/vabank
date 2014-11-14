using Autofac;
using System;
using AutoMapper;
using VaBank.Common.Events;
using VaBank.Jobs.Common;
using VaBank.Jobs.Common.Settings;

namespace VaBank.Jobs.Modules
{
    public class BackgroundServicesModule : Module
    {
        private readonly IServiceBus _serviceBus;

        public BackgroundServicesModule(IServiceBus serviceBus)
        {
            if (serviceBus == null)
            {
                throw new ArgumentNullException("serviceBus");
            }
            _serviceBus = serviceBus;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //Modules
            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<DataAccessModule>();
            builder.RegisterModule<ServicesModule>();

            //Automapper profiles
            builder.RegisterAssemblyTypes(typeof(BaseJob<>).Assembly)
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract)
                .As<Profile>()
                .SingleInstance();

            builder.RegisterInstance(_serviceBus)
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(typeof (IJobContext).IsAssignableFrom)
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DefaultJobContext<>))
                .As(typeof(IJobContext<>))
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(typeof (IJob).IsAssignableFrom)
                .Where(x => !x.IsAbstract)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<JobSettingsProvider>().AsSelf().InstancePerDependency();
            builder.RegisterType<JobStartup>().AsSelf().SingleInstance();
        }
    }
}