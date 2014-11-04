using Autofac;
using VaBank.Common.Events;

namespace VaBank.Services.Tests.Modules
{
    public class ServiceTestModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<DataAccessModule>();
            builder.RegisterModule<ServicesModule>();

            builder.RegisterInstance(TestServiceBus.Instance)
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
