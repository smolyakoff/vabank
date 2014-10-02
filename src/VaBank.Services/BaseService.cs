using FluentValidation.Results;
using System;
using System.Linq;
using System.Threading.Tasks;
using VaBank.Services.Contracts;
using VaBank.Services.Contracts.Validation;

namespace VaBank.Services
{
    public abstract class BaseService : IService
    {
        private readonly IValidationFactory _validationFactory;
        
        protected BaseService(IValidationFactory validationFactory)
        {
            if (validationFactory == null)
                throw new ArgumentNullException("validationFactory", "Validation factory can't be null");
            _validationFactory = validationFactory;
        }

        public void EnsureIsValid<T>(T obj)
        {
            var validator = _validationFactory.GetValidator<T>();
            var validationResult = validator.Validate(obj);
            if (!validationResult.IsValid)
            {
                var faults = validationResult.Errors.Select(x => new ValidationFault(x.PropertyName, x.ErrorMessage)).ToList();
                throw new ValidationException("Object has validation errors. See ValidationFaults property for more information.", faults);
            }
        }

        public ValidationResult Validate<T>(T obj)
        {
            var validator = _validationFactory.GetValidator<T>();
            return validator.Validate(obj);
        }

        public Task<ValidationResult> ValidateAsync<T>(T obj)
        {
            var validator = _validationFactory.GetValidator<T>();
            return validator.ValidateAsync(obj);
        } 
    }
}
