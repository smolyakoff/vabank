using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VaBank.Common.Data;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Validation;

namespace VaBank.Services.Common.Validation
{
    public class ValidationService : IValidationService
    {
        private static readonly Dictionary<string, Type> Validators;

        static ValidationService()
        {
            var validators = Assembly.GetExecutingAssembly().GetTypes()
                .Union(typeof(Entity).Assembly.GetTypes())
                .Where(t => t.IsDefined(typeof (ValidatorNameAttribute), false))
                .Where(t => IsSubclassOfRawGeneric(typeof(ObjectValidator<>), t))
                .Select(t => new {type = t, name = t.GetCustomAttribute<ValidatorNameAttribute>().Name})
                .ToDictionary(x => x.name, x => x.type, StringComparer.OrdinalIgnoreCase);
            Validators = validators;
        }

        private readonly IObjectFactory _objectFactory;

        private readonly IObjectConverter _objectConverter;

        public ValidationService(IObjectFactory objectFactory, IObjectConverter objectConverter)
        {

            if (objectFactory == null)
            {
                throw new ArgumentNullException("objectFactory", "Object factory should not be null.");
            }
            if (objectConverter == null)
            {
                throw new ArgumentNullException("objectConverter", "Object converter should not be null.");
            }
            _objectFactory = objectFactory;
            _objectConverter = objectConverter;
        }

        public ValidationResponse Validate(ValidationRequest validationRequest)
        {
            if (validationRequest == null)
            {
                throw new ArgumentNullException("validationRequest");
            }
            if (!Validators.ContainsKey(validationRequest.ValidatorName))
            {
                return new ValidationResponse {IsValidatorFound = false};
            }
            try
            {
                var validatorType = Validators[validationRequest.ValidatorName];
                var validator = _objectFactory.Create(validatorType) as IObjectValidator;
                if (validator == null)
                {
                    throw new InvalidOperationException("Validator is null.");
                }
                var objectToValidate = _objectConverter.Convert(validationRequest.Value, validator.ValidatedType);
                IList<ValidationFault> result = validator.Validate(objectToValidate);
                return new ValidationResponse
                {
                    ValidationFaults = result.Select(x => new ValidationFault(validationRequest.ValidatorName, x.Message)).ToList(),
                    IsValidatorFound = true
                };
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't validate requested object", ex);
            }
            
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof (object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
