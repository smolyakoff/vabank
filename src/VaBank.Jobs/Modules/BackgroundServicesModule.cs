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
                .Where(typeof (IJob).IsAssignableFrom)
                .AsSelf()
                .InstancePerDependency();
            builder.RegisterType<JobExecutor>().AsSelf().SingleInstance();
        }
    }
}