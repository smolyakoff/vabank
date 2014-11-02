using System;
using Autofac;

namespace VaBank.Common.IoC
{
    public class AutofacFactory : IObjectFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacFactory(ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope == null)
                throw new ArgumentNullException("lifetimeScope", "Lifetime Scope can't be null");
            _lifetimeScope = lifetimeScope;
        }

        public object Create(Type objectType)
        {
            return _lifetimeScope.Resolve(objectType);
        }

        public T Create<T>()
        {
            return _lifetimeScope.Resolve<T>();
        }

        public bool CanCreate<T>()
        {
            return CanCreate(typeof (T));
        }

        public bool CanCreate(Type objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException("objectType");
            }
            return _lifetimeScope.IsRegistered(objectType);
        }
    }
}