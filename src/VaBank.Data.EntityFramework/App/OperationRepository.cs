using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using VaBank.Common.Data.Repositories;
using VaBank.Core.App;
using VaBank.Data.EntityFramework.Membership.Mappings;

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
                var tempId = Guid.NewGuid();
                var timestamp = DateTime.UtcNow;
                var operation = new Operation(tempId, timestamp, name, identity);
                var operationId = new SqlParameter("@Id", SqlDbType.UniqueIdentifier) {Direction = ParameterDirection.Output};
                var userId = new SqlParameter("@AppUserId", (object)operation.UserId ?? DBNull.Value) {DbType = DbType.Guid};
                var clientId = new SqlParameter("@AppClientId", (object)operation.ClientApplicationId ?? DBNull.Value);
                var timestampUtc = new SqlParameter("@TimestampUtc", timestamp);
                var nameSql = new SqlParameter("@Name", SqlDbType.NVarChar, RestrictionConstants.NameLength) {Value = (object)name ?? DBNull.Value};
                const string sql =
                    @"EXEC [App].[StartOperation] @Id = @Id OUTPUT, @TimestampUtc = @TimestampUtc, @Name = @Name, @AppUserId = @AppUserId, @AppClientId = @AppClientId";
                Context.Database.ExecuteSqlCommand(sql, operationId, userId, clientId, timestampUtc, nameSql);
                var idProperty = typeof (Operation).GetProperty("Id");
                idProperty.SetValue(operation, (Guid)operationId.Value);
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

        public bool HasCurrent
        {
            get { return GetCurrent() != null; }
        }

        public Operation GetCurrent()
        {
            try
            {
                const string sql = 
                    "SELECT [Id], [Name], [TimestampUtc], [AppUserId] as [UserId], [AppClientId] as [ClientApplicationId] FROM [App].[CurrentOperation]";
                return Context.Database.SqlQuery<Operation>(sql).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can't release operation marker from the db.", ex);
            }
        }
    }
}
