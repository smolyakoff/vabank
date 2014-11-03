using System;
using FluentValidation;
using VaBank.Common.Data.Database;
using VaBank.Common.Events;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.App.Entities;
using VaBank.Core.Common;
using VaBank.Services.Common.Security;
using VaBank.Services.Common.Transactions;
using VaBank.Services.Contracts;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Common.Security;
using VaBank.Services.Contracts.Common.Validation;
using ValidationException = VaBank.Services.Contracts.Common.Validation.ValidationException;

namespace VaBank.Services.Common
{
    public abstract class BaseService : IService
    {
        private readonly IObjectFactory _objectFactory;
        private readonly ISendOnlyServiceBus _bus;
        private readonly ServiceOperationProvider _operationProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionProvider _transactionProvider;
        private VaBankIdentity _identity = null;

        protected BaseService(BaseServiceDependencies dependencies)
        {
            if (dependencies == null)
            {
                throw new ArgumentNullException("dependencies", "Dependencies should be resolved");
            }
            dependencies.EnsureIsResolved();
            _objectFactory = dependencies.ObjectFactory;
            _operationProvider = dependencies.OperationProvider;
            _bus = dependencies.ServiceBus;
            _unitOfWork = dependencies.UnitOfWork;
            _transactionProvider = dependencies.TransactionProvider;
        }

        protected virtual void EnsureIsSecure<T>(T obj)
        {
            var validatorType = typeof(ISecurityValidator<T>);
            EnsureIsSecure(obj, validatorType);
        }

        protected virtual void EnsureIsSecure<T, TValidator>(T obj)
            where TValidator : ISecurityValidator<T>
        {
            EnsureIsSecure(obj, typeof(TValidator));
        }

        protected virtual void EnsureIsSecure<T, TValidator>(T obj, TValidator validator)
            where TValidator : class, ISecurityValidator<T>
        {
            Argument.NotNull(validator, "validator");
            var validationResult = validator.Validate(obj);
            if (!validationResult.IsValid)
            {
                throw new SecurityValidatorException(validationResult.Errors);
            }
        }

        protected virtual void EnsureIsValid<T>(T obj)
        {
            var validatorType = typeof (IValidator<T>);
            EnsureIsValid(obj, validatorType);
        }

        protected virtual void EnsureIsValid<T, TValidator>(T obj)
            where TValidator : IValidator<T>
        {
            EnsureIsValid(obj, typeof (TValidator));
        }

        protected VaBankIdentity Identity
        {
            get { return _identity ?? (_identity = _objectFactory.Create<VaBankIdentity>()); }
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

        protected virtual void Commit(CommitMode commitMode = CommitMode.Auto)
        {
            if (_transactionProvider.HasCurrentTransaction || commitMode == CommitMode.EnsureOperation)
            {
                _operationProvider.GetCurrent();
            }
            _unitOfWork.Commit();
        }

        protected virtual void Publish(ApplicationEvent appEvent)
        {
            _bus.Publish(appEvent);
        }

        private void EnsureIsSecure<T>(T obj, Type validatorType)
        {
            if (!_objectFactory.CanCreate(validatorType))
            {
                return;
            }
            var validator = _objectFactory.Create(validatorType) as ISecurityValidator<T>;
            if (validator == null)
            {
                throw new InvalidOperationException("Object factory returned null for security validator.");
            }
            var validationResult = validator.Validate(obj);
            if (!validationResult.IsValid)
            {
                throw new SecurityValidatorException(validationResult.Errors);
            }
        }

        private void EnsureIsValid<T>(T obj, Type validatorType)
        {
            if (!_objectFactory.CanCreate(validatorType))
            {
                return;
            }
            var validator = _objectFactory.Create(validatorType) as IValidator<T>;
            if (validator == null)
            {
                throw new InvalidOperationException("Object factory returned null for validator.");
            }
            var validationResult = validator.Validate(obj);
            if (!validationResult.IsValid)
            {
                var faults = validationResult.Errors.ToValidationFaults();
                throw new ValidationException(
                    "Object has validation errors. See ValidationFaults property for more information.", faults);
            }
        }
    }
}
