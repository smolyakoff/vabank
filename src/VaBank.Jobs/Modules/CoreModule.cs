using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Autofac;
using VaBank.Common.IoC;
using VaBank.Common.Util;
using VaBank.Core.Common;
using Module = Autofac.Module;

namespace VaBank.Jobs.Modules
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            LoadInjectables(builder);
            LoadSettings(builder);
        }

        private static void LoadInjectables(ContainerBuilder builder)
        {
            var injectables = typeof(Entity).Assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<InjectableAttribute>() != null)
                .Select(t => new { Lifetime = t.GetCustomAttribute<InjectableAttribute>().Lifetime, Type = t })
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

        private static void LoadSettings(ContainerBuilder builder)
        {
            var settings = typeof(Entity).Assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<SettingsAttribute>() != null)
                .ToList();
            foreach (var setting in settings)
            {
                var copied = setting;
                builder.Register(c => c.Resolve<SettingsManager>().Load(copied))
                    .As(copied)
                    .InstancePerLifetimeScope();
            }
        }
    }
}