using Autofac;
using VaBank.Services.Tests.Fakes;

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
            builder.RegisterType<TestStartup>()
                .AsImplementedInterfaces()
                .SingleInstance();

        }
    }
}
