using System;
using Autofac;
using FluentValidation;
using VaBank.Services.Validation;

namespace VaBank.UI.Web.Validation
{
    public class DefaultValidationFactory : IValidationFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public DefaultValidationFactory(ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope == null)
                throw new ArgumentNullException("lifetimeScope", "Lifetime Scope can't be null");
            _lifetimeScope = lifetimeScope;
        }

        public AbstractValidator<T> GetValidator<T>()
        {
            return _lifetimeScope.Resolve<AbstractValidator<T>>();
        }
    }
}