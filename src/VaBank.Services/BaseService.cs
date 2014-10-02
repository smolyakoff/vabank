using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using VaBank.Services.Contracts;
using VaBank.Services.Validation;

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
