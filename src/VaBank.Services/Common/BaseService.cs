using System;
using System.Linq;
using FluentValidation;
using VaBank.Common.Events;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Services.Contracts;
using VaBank.Services.Contracts.Events;
using ValidationException = VaBank.Services.Contracts.Common.Validation.ValidationException;

namespace VaBank.Services.Common
{
    public abstract class BaseService : IService
    {
        private readonly IValidatorFactory _validatorFactory;
        private readonly ISendOnlyServiceBus _bus;

        protected readonly IUnitOfWork UnitOfWork;
        

        //TODO: refactor this to use object factory instead of validator factory
        protected BaseService(IUnitOfWork unitOfWork, IValidatorFactory validatorFactory, ISendOnlyServiceBus bus)
        {
            if (validatorFactory == null)
            {
                throw new ArgumentNullException("validatorFactory", "Validator factory can't be null");
            }
            _validatorFactory = validatorFactory;
            UnitOfWork = unitOfWork;
            _bus = bus;
        }

        protected virtual void EnsureIsValid<T>(T obj)
        {
            var validator = _validatorFactory.GetValidator<T>();
            var validationResult = validator.Validate(obj);
            if (!validationResult.IsValid)
            {
                var faults = validationResult.Errors.Select(x => new ValidationFault(x.PropertyName, x.ErrorMessage)).ToList();
                throw new ValidationException("Object has validation errors. See ValidationFaults property for more information.", faults);
            }
        }

        protected virtual void Publish(ApplicationEvent appEvent)
        {
            _bus.Publish(appEvent);
        }
    }
}
