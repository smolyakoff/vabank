﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VaBank.Common.Data;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Infrastructure.Validation;

namespace VaBank.Services.Infrastructure.Validation
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

        public ValidationResultModel Validate(ValidationCommand validationCommand)
        {
            if (validationCommand == null)
            {
                throw new ArgumentNullException("validationCommand");
            }
            if (!Validators.ContainsKey(validationCommand.ValidatorName))
            {
                return new ValidationResultModel {IsValidatorFound = false};
            }
            try
            {
                var validatorType = Validators[validationCommand.ValidatorName];
                var validator = _objectFactory.Create(validatorType) as IObjectValidator;
                if (validator == null)
                {
                    throw new InvalidOperationException("Validator is null.");
                }
                var objectToValidate = _objectConverter.Convert(validationCommand.Value, validator.ValidatedType);
                IList<ValidationFault> result = validator.Validate(objectToValidate);
                return new ValidationResultModel
                {
                    ValidationFaults = result.Select(x => new ValidationFault(validationCommand.ValidatorName, x.Message)).ToList(),
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