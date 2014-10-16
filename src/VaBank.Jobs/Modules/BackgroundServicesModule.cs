using Autofac;
using VaBank.Jobs.Common;

namespace VaBank.Jobs.Modules
{
    public class BackgroundServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DataAccessModule>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(typeof (IJobContext).IsAssignableFrom)
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DefaultJobContext<>))
                .As(typeof(IJobContext<>))
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(typeof (IJob).IsAssignableFrom)
                .AsSelf()
                .InstancePerDependency();
        }
    }
}