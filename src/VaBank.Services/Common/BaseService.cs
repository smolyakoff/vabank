using System;
using System.Linq;
using FluentValidation;
using VaBank.Common.Validation;
using VaBank.Core.App;
using VaBank.Core.Common;
using VaBank.Services.Contracts;
using ValidationException = VaBank.Services.Contracts.Common.Validation.ValidationException;

namespace VaBank.Services.Common
{
    public abstract class BaseService : IService
    {
        private readonly IValidatorFactory _validatorFactory;

        private readonly ServiceOperationProvider _operationProvider;

        protected readonly IUnitOfWork UnitOfWork;

        //TODO: refactor this to use object factory instead of validator factory
        protected BaseService(BaseServiceDependencies dependencies)
        {
            if (dependencies == null)
            {
                throw new ArgumentNullException("dependencies", "Dependencies should be resolved");
            }
            dependencies.EnsureIsResolved();
            _validatorFactory = dependencies.ValidatorFactory;
            _operationProvider = dependencies.OperationProvider;
            UnitOfWork = dependencies.UnitOfWork;
        }

        protected virtual void EnsureIsValid<T>(T obj)
        {
            var validator = _validatorFactory.GetValidator<T>();
            var validationResult = validator.Validate(obj);
            if (!validationResult.IsValid)
            {
                var faults =
                    validationResult.Errors.Select(x => new ValidationFault(x.PropertyName, x.ErrorMessage)).ToList();
                throw new ValidationException(
                    "Object has validation errors. See ValidationFaults property for more information.", faults);
            }
        }

        protected Operation Operation
        {
            get
            {
                var operation = _operationProvider.GetCurrent();
                if (operation == null)
                {
                    throw new InvalidOperationException("Operation provider returned null.");
                }
                return operation;
            }
        }
    }
}
