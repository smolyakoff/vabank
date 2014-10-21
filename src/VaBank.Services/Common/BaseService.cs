using System;
using System.Linq;
using FluentValidation;
using VaBank.Common.Events;
using VaBank.Common.Validation;
using VaBank.Core.App;
using VaBank.Core.Common;
using VaBank.Services.Contracts;
using VaBank.Services.Contracts.Common;
using ValidationException = VaBank.Services.Contracts.Common.Validation.ValidationException;
using VaBank.Common.IoC;

namespace VaBank.Services.Common
{
    public abstract class BaseService : IService
    {
        private readonly IObjectFactory _objectFactory;
        private readonly ISendOnlyServiceBus _bus;
        private readonly ServiceOperationProvider _operationProvider;

        protected readonly IUnitOfWork UnitOfWork;
        
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
            UnitOfWork = dependencies.UnitOfWork;
        }

        protected virtual void EnsureIsValid<T>(T obj)
        {
            var validator = _objectFactory.Create<IValidator<T>>();
            if (validator == null) return;
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

        protected virtual void Publish(ApplicationEvent appEvent)
        {
            _bus.Publish(appEvent);
        }
    }
}
