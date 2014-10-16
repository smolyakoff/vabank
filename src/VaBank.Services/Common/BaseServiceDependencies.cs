using FluentValidation;
using VaBank.Common.Events;
using VaBank.Core.Common;

namespace VaBank.Services.Common
{
    public class BaseServiceDependencies : IDependencyCollection
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public IValidatorFactory ValidatorFactory { get; set; }

        public ServiceOperationProvider OperationProvider { get; set; }

        public ISendOnlyServiceBus ServiceBus { get; set; }
    }
}
