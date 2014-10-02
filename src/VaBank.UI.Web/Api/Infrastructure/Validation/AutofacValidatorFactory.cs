using System;
using Autofac;
using FluentValidation;
using VaBank.Services.Contracts.Common.Validation;
using IValidatorFactory = VaBank.Services.Contracts.Common.Validation.IValidatorFactory;

namespace VaBank.UI.Web.Api.Infrastructure.Validation
{
    public class AutofacValidatorFactory : IValidatorFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacValidatorFactory(ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope == null)
                throw new ArgumentNullException("lifetimeScope", "Lifetime Scope can't be null");
            _lifetimeScope = lifetimeScope;
        }

        public IValidator<T> GetValidator<T>()
        {
            return _lifetimeScope.IsRegistered<IValidator<T>>() 
                ? _lifetimeScope.Resolve<IValidator<T>>() 
                : new AlwaysTrueValidator<T>();
        }
    }
}