using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using VaBank.Common.Data.Repositories;
using VaBank.Core.App;

namespace VaBank.Data.EntityFramework.App
{
    public class OperationMarkerRepository : IRepository, IOperationMarkerRepository, IOperationMarkerProvider
    {
        protected readonly DbContext Context;

        public OperationMarkerRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "DbContext should not be empty");
            }
            Context = context;
        }

        public OperationMarker Take(string name, Guid? userId, string clientId)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            try
            {
                var param = new SqlParameter() {Direction = ParameterDirection.Output, ParameterName = "@Id", DbType = DbType.Guid};
                Context.Database.ExecuteSqlCommand(
                    "EXEC [App].[StartOperationMarker] @Id = @Id OUTPUT, @TimestampUtc = NULL, @Name = NULL, @AppUserId = NULL, @AppClientId = NULL",
                    param);
                var marker = new OperationMarker((Guid)param.Value, name, userId, clientId);
                return marker;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can't take operation marker from the db.", ex);
            }
        }

        public void Release(OperationMarker marker)
        {
            if (marker == null)
            {
                throw new ArgumentNullException("marker");
            }
            try
            {
                //var dbId = new SqlParameter("@Id", SqlDbType.UniqueIdentifier) {Value = marker.Id};
                Context.Database.ExecuteSqlCommand("EXEC [App].[FinishOperationMarker]");
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can't release operation marker from the db.", ex);
            }
        }

        public OperationMarker Get()
        {
            try
            {
                return Context.Database.SqlQuery<OperationMarker>(
                   "SELECT [Id], [Name], [AppUserId] as [UserId], [AppClientId] as [ClientName] FROM [App].[GetOperationMarker]()").FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can't release operation marker from the db.", ex);
            }
        }
    }
}
