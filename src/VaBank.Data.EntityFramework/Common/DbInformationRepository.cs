using System.Data.Entity;
using System.Linq;
using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Data.EntityFramework.Common
{
    public class DbInformationRepository : IDbInformationRepository
    {
        private readonly DbContext _context;

        public DbInformationRepository(DbContext context)
        {
            Argument.NotNull(context, "context");
            _context = context;
        }

        public long GetDbVersion()
        {
            return _context.Database.SqlQuery<long>("SELECT MAX([Version]) FROM [Maintenance].[DbVersions]").First();
        }
    }
}
