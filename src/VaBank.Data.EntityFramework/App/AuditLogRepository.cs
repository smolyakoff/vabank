using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Core.App;

namespace VaBank.Data.EntityFramework.App
{
    public class AuditLogRepository : IRepository, IAuditLogRepository
    {
        private readonly DbContext _dbContext;

        public AuditLogRepository(DbContext dbContext)
        {
           if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            _dbContext = dbContext;
        }

        public IList<AuditLogBriefEntry> GetAuditEntries(DbQuery<ApplicationAction> query)
        {
            if (query == null)
                throw new ArgumentNullException("query");
            return EnsureRepositoryException(() =>
            {
                var actions = _dbContext.Set<ApplicationAction>().Query(query).ToList();
                var operationIds = actions.Select(x => x.OperationId).ToList();
                var operations = _dbContext.Set<Operation>().Where(x => operationIds.Contains(x.Id)).ToList();
                var entries = operations.Select(operation => new AuditLogBriefEntry(operation)
                {
                    ApplicationActions = actions.Where(x => x.OperationId == operation.Id).ToList()
                }).ToList();
                return entries;
            });
        }

        public AuditLogEntry GetAuditEntryDetails(Guid operationId)
        {
            var operation = _dbContext.Set<Operation>().Find(operationId);
            if (operation == null)
                throw new InvalidOperationException(string.Format("Operation with {0} id not found",
                    operationId.ToString()));

            return EnsureRepositoryException(() =>
            {
                var actions = _dbContext.Set<ApplicationAction>().Where(x => x.OperationId == operation.Id).ToList();
                var auditTableNames = GetTableNames();
                var connection = _dbContext.Database.Connection;
                var command = connection.CreateCommand();
                var sqlParameter = new SqlParameter("@OperationId", SqlDbType.UniqueIdentifier)
                {
                    Value = operation.Id
                };
                command.Parameters.Add(sqlParameter);

                var dbActions = new List<DatabaseAction>();
                try
                {
                    if (connection.State == ConnectionState.Broken)
                    {
                        connection.Close();
                        connection.Open();
                    }
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    
                    foreach (var tableName in auditTableNames)
                    {
                        command.CommandText = string.Format(
                            "SELECT * {0} WHERE HistoryOperationId = @OperationId",
                            tableName.GetFullTableName());
                        var reader = command.ExecuteReader();
                        var dbAction = new DatabaseAction
                        {
                            TableName = tableName.GetFullTableName(),
                            ChangedRows = new List<VersionedDatabaseRow>()
                        };
                        dbActions.Add(dbAction);

                        while (reader.Read())
                        {
                            var values = new Dictionary<string, object>();
                            var dbRow = new VersionedDatabaseRow(values);
                            dbAction.ChangedRows.Add(dbRow);

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                if (columnName == "HistoryId")
                                {
                                    dbRow.Version = reader.GetInt64(i);
                                    continue;
                                }
                                if (columnName == "HistoryTimestampUtc")
                                {
                                    dbRow.TimestampUtc = reader.GetDateTime(i);
                                    continue;
                                }
                                if (columnName == "HistoryAction")
                                {
                                    dbRow.Action = reader.GetChar(i).ToOperation();
                                    continue;
                                }
                                values.Add(columnName, reader.GetValue(i));
                            }
                        }
                    }

                    return new AuditLogEntry(operation)
                    {
                        ApplicationActions = actions,
                        DatabaseActions = dbActions
                    };
                }
                finally
                {
                    connection.Close();
                }
            });
        }

        private IEnumerable<TableNamePair> GetTableNames()
        {
            const string sql =
                @"SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME LIKE '%[_]History'";

            var auditTableNames = _dbContext.Database.SqlQuery<TableNamePair>(sql).ToList();
            return auditTableNames;
        }

        public IList<string> GetUniqueCodes()
        {
            return
                EnsureRepositoryException(
                    () => _dbContext.Set<ApplicationAction>().Select(x => x.Code).Distinct().ToList());
        }

        public void CreateAction(ApplicationAction action)
        {
            EnsureRepositoryException(() => _dbContext.Set<ApplicationAction>().Add(action));
        }

        protected T EnsureRepositoryException<T>(Func<T> call)
        {
            try
            {
                return call();
            }
            catch (RepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }
    }

    internal class TableNamePair
    {
        public string TableSchema { get; set; }

        public string TableName { get; set; }

        public string GetFullTableName()
        {
            return string.Format("[{0}].[{1}]", TableSchema, TableName);
        }
    }
}
