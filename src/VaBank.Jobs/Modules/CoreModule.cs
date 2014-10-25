using System.Linq;
using System.Reflection;
using Autofac;
using VaBank.Common.IoC;
using VaBank.Core.Common;
using Module = Autofac.Module;

namespace VaBank.Jobs.Modules
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var injectables = typeof (Entity).Assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<InjectableAttribute>() != null)
                .Select(t => new {Lifetime = t.GetCustomAttribute<InjectableAttribute>().Lifetime, Type = t})
                .ToList();

            foreach (var injectable in injectables)
            {
                var registration = builder.RegisterType(injectable.Type).AsImplementedInterfaces().AsSelf();
                switch (injectable.Lifetime)
                {
                    case Lifetime.Singleton:
                        registration.SingleInstance();
                        break;
                    case Lifetime.PerDependency:
                        registration.InstancePerDependency();
                        break;
                }
            }
        }
    }
}