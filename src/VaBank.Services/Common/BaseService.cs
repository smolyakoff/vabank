using System;
using System.Linq;using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using VaBank.Core.Common;
using VaBank.Services.Contracts;
using VaBank.Services.Contracts.Common.Validation;
using ValidationException = VaBank.Services.Contracts.Common.Validation.ValidationException;

namespace VaBank.Services.Common
{
    public abstract class BaseService : IService
    {
        private readonly IValidatorFactory _validatorFactory;

        protected readonly IUnitOfWork UnitOfWork;
        
        protected BaseService(IUnitOfWork unitOfWork, IValidatorFactory validatorFactory)
        {
            if (validatorFactory == null)
            {
                throw new ArgumentNullException("validatorFactory", "Validator factory can't be null");
            }
            _validatorFactory = validatorFactory;
            UnitOfWork = unitOfWork;
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

        public ValidationResult Validate<T>(T obj)
        {
            var validator = _validatorFactory.GetValidator<T>();
            return validator.Validate(obj);
        }

        public Task<ValidationResult> ValidateAsync<T>(T obj)
        {
            var validator = _validatorFactory.GetValidator<T>();
            return validator.ValidateAsync(obj);
        } 
    }
}
