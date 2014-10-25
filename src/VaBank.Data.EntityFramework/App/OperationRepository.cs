using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using VaBank.Common.Data.Repositories;
using VaBank.Core.App;
using VaBank.Core.App.Entities;
using VaBank.Core.App.Providers;
using VaBank.Core.App.Repositories;
using VaBank.Core.Membership;
using VaBank.Core.Membership.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.App
{
    public class OperationRepository : IRepository, IOperationRepository, IOperationProvider
    {
        protected readonly DbContext Context;

        public OperationRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "DbContext should not be empty");
            }
            Context = context;
        }

        public Operation Start(string name, ClaimsIdentity identity)
        {
            try
            {
                var timestamp = DateTime.UtcNow;
                identity = identity ?? new ClaimsIdentity();
                var sid = identity.FindFirst(UserClaim.Types.UserId);
                var userId = sid == null ? null : (Guid?)Guid.Parse(sid.Value);
                var clientIdClaim = identity.FindFirst(UserClaim.Types.ClientId);
                var clientId = clientIdClaim == null ? null : clientIdClaim.Value;
                var operationId = new SqlParameter("@Id", SqlDbType.UniqueIdentifier) {Direction = ParameterDirection.Output};
                var userIdSql = new SqlParameter("@AppUserId", (object)userId ?? DBNull.Value) {DbType = DbType.Guid};
                var appClientIdSql = new SqlParameter("@AppClientId", (object)clientId ?? DBNull.Value);
                var startedUtc = new SqlParameter("@StartedUtc", timestamp);
                var nameSql = new SqlParameter("@Name", SqlDbType.NVarChar, Restrict.Length.Name) {Value = (object)name ?? DBNull.Value};
                const string sql =
                    @"EXEC [App].[StartOperation] @Id = @Id OUTPUT, @StartedUtc = @StartedUtc, @Name = @Name, @AppUserId = @AppUserId, @AppClientId = @AppClientId";
                Context.Database.ExecuteSqlCommand(sql, operationId, userIdSql, appClientIdSql, startedUtc, nameSql);
                var operation = Context.Set<Operation>().Find(operationId.Value);
                return operation;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can't take operation marker from the db.", ex);
            }
        }

        public void Stop(Operation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }
            try
            {
                var operationId = new SqlParameter("@Id", operation.Id);
                Context.Database.ExecuteSqlCommand("EXEC [App].[FinishOperation] @Id", operationId);
                ((IDisposable)operation).Dispose();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can't release operation marker from the db.", ex);
            }
        }

        public Operation Find(Guid operationId)
        {
            try
            {
                return Context.Set<Operation>().Find(operationId);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can't find operation in the db.", ex);
            }
        }

        public bool HasCurrent
        {
            get { return GetCurrent() != null; }
        }

        public Operation GetCurrent()
        {
            try
            {
                const string sql = "SELECT [Id] FROM [App].[CurrentOperation]";
                var id = Context.Database.SqlQuery<Guid>(sql).FirstOrDefault();
                return id == Guid.Empty ? null : Context.Set<Operation>().Find(id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can't release operation marker from the db.", ex);
            }
        }
    }
}
