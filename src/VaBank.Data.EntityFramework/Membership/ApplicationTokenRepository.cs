using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Membership;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Membership
{
    public class ApplicationTokenRepository : Repository<ApplicationToken>, IApplicationTokenRepository
    {
        public ApplicationTokenRepository(DbContext context) : base(context)
        {
        }

        public ApplicationToken GetAndDelete(string id)
        {
            try
            {
                return Context.Set<ApplicationToken>()
                    .SqlQuery("DELETE [Membership].[ApplicationToken] OUTPUT DELETED.* WHERE [Id] = @id", new SqlParameter("id", id))
                    .AsNoTracking()
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can't get and delete token", ex);
            }
        }
    }
}
