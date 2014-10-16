using System.Linq;
using Autofac;
using VaBank.Common.Events;

namespace VaBank.UI.Web.Modules
{
    public class HangfireBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = GetType().Assembly;
            var handlers =
                assembly.GetTypes()
                    .Where(
                        x =>
                            x.GetInterfaces()
                                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IHandler<>)))
                    .ToArray();
            builder.RegisterTypes(handlers)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
    }
}