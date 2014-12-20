using VaBank.Common.Data.Repositories;
using VaBank.Core.Common;
using VaBank.Core.Membership.Entities;
using VaBank.Services.Common;

namespace VaBank.Services.Statistics
{
    public class VaBankStatisticsServiceDependencies : IDependencyCollection
    {
        public IRepository<User> Users { get; set; }

        public IDbInformationRepository DbInformation { get; set; }
    }
}
