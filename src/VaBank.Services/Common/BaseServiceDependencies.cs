using VaBank.Common.Data.Database;
using VaBank.Common.Events;
using VaBank.Common.IoC;
using VaBank.Core.Common;

namespace VaBank.Services.Common
{
    public class BaseServiceDependencies : IDependencyCollection
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public IObjectFactory ObjectFactory { get; set; }

        public ServiceOperationProvider OperationProvider { get; set; }

        public ISendOnlyServiceBus ServiceBus { get; set; }

        public ITransactionProvider TransactionProvider { get; set; }
    }
}
