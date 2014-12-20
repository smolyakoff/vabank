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

        public string GetServerVersion()
        {
            return _context.Database.SqlQuery<string>("SELECT @@VERSION").First();
        }
    }
}
