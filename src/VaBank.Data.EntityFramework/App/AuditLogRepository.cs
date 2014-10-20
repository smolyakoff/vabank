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
        //TODO: можно объеденить в private static class 
        private const string VersionColumnName = "HistoryId";
        private const string TimestampUtcColumnName = "HistoryTimestampUtc";
        private const string ActionColumnName = "HistoryAction";

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
                var actions = _dbContext.Set<ApplicationAction>().Query(query).ToLookup(x => x.Operation).ToList();
                var entries = actions.Select(x => new AuditLogBriefEntry(x.Key, x));
                return entries.ToList();
            });
        }

        public AuditLogEntry GetAuditEntryDetails(Guid operationId)
        {
            var operation = _dbContext.Set<Operation>().Find(operationId);
            if (operation == null)
                return null;

            return EnsureRepositoryException(() =>
            {
                var actions = _dbContext.Set<ApplicationAction>().Where(x => x.Operation.Id == operation.Id).ToList();
                var auditTableNames = GetHistoryTableNames();
                var connection = _dbContext.Database.Connection;
                var command = connection.CreateCommand();
                var sqlParameter = new SqlParameter("@OperationId", SqlDbType.UniqueIdentifier)
                {
                    Value = operation.Id
                };
                command.Parameters.Add(sqlParameter);

                var dbActions = new List<DatabaseAction>();

                
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

                    //TODO: хз, я бы использовал DataTable.Fill()
                    while (reader.Read())
                    {
                        var values = new Dictionary<string, object>();
                        var dbRow = new VersionedDatabaseRow(values);

                        dbAction.ChangedRows.Add(dbRow);

                        dbRow.Version = (long)reader[VersionColumnName];
                        dbRow.TimestampUtc = (DateTime)reader[TimestampUtcColumnName];
                        dbRow.Action = ToOperation(((char)reader[ActionColumnName]));

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            //TODO: а если десять колонок будет - десять условий будешь писать?
                            if (columnName == VersionColumnName || columnName == TimestampUtcColumnName ||
                                columnName == ActionColumnName)
                            {
                                continue;
                            }
                            values.Add(columnName, reader.GetValue(i));
                        }
                    }
                }
                //TODO: DatabaseActions сеттер должен быть протектек или прайвит
                return new AuditLogEntry(operation, actions)
                {
                    DatabaseActions = dbActions
                };
            });
        }

        private IEnumerable<TableNamePair> GetHistoryTableNames()
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

        public static DatabaseOperation ToOperation(char value)
        {
            switch (value)
            {
                case 'U':
                    return DatabaseOperation.Update;
                case 'D':
                    return DatabaseOperation.Delete;
                case 'I':
                    return DatabaseOperation.Insert;
                default:
                    throw new InvalidOperationException("Can't convert char value to DatabaseOperation");
            }
        }
    }

    //TODO: этот класс может быть private внутри репозитория
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
