using System;
using Autofac;
using VaBank.Common.Events;
using VaBank.Jobs.Common;

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
            builder.RegisterModule<DataAccessModule>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterInstance(_serviceBus)
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(typeof (IJobContext).IsAssignableFrom)
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DefaultJobContext<>))
                .As(typeof(IJobContext<>))
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(typeof (IJob).IsAssignableFrom)
                .Where(x => !x.IsAbstract)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<AfterLoad>().AutoActivate();
        }
    }
}