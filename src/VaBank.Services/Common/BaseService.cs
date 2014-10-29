using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using FluentValidation;
using VaBank.Common.Data.Database;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Events;
using VaBank.Common.Validation;
using VaBank.Core.App;
using VaBank.Core.App.Entities;
using VaBank.Core.Common;
using VaBank.Core.Membership;
using VaBank.Services.Contracts;
using VaBank.Services.Contracts.Common.Events;
using ValidationException = VaBank.Services.Contracts.Common.Validation.ValidationException;
using VaBank.Common.IoC;

namespace VaBank.Services.Common
{
    public abstract class BaseService : IService
    {
        private readonly IObjectFactory _objectFactory;
        private readonly ISendOnlyServiceBus _bus;
        private readonly ServiceOperationProvider _operationProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionProvider _transactionProvider;
        private readonly VaBankIdentity _identity;
        
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
            _identity = new VaBankIdentity(Thread.CurrentPrincipal.Identity as ClaimsIdentity, dependencies.UserRepository);
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

        protected VaBankIdentity Identity
        {
            get { return _identity; }
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
    }
}
