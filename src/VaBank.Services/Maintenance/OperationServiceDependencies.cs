using VaBank.Core.App;
using VaBank.Core.App.Repositories;
using VaBank.Services.Common;
using VaBank.Services.Common.Transactions;

namespace VaBank.Services.Maintenance
{
    public class OperationServiceDependencies : IDependencyCollection
    {
        public ServiceOperationProvider Provider { get; set; }

        public IOperationRepository Repository { get; set; }
    }
}
