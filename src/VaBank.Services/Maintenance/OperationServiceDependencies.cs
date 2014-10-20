using VaBank.Core.App;
using VaBank.Services.Common;

namespace VaBank.Services.Maintenance
{
    public class OperationServiceDependencies : IDependencyCollection
    {
        public ServiceOperationProvider Provider { get; set; }

        public IOperationRepository Repository { get; set; }
    }
}
