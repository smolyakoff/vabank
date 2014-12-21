using VaBank.Common.Data.Repositories;
using VaBank.Core.Common;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;

namespace VaBank.Services.Maintenance
{
    public class SystemStatisticsServiceDependencies : IDependencyCollection
    {
        public IQueryRepository<User> Users { get; set; }

        public IDbInformationRepository DbInformation { get; set; }

        public IQueryRepository<Transaction> Transactions { get; set; } 

        public IQueryRepository<BankOperation> BankOperations { get; set; }
    }
}
