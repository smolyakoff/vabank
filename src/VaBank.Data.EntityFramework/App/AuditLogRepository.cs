using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using AutoMapper.QueryableExtensions;
using VaBank.Common.Data;
using VaBank.Common.Data.Database;
using VaBank.Common.Data.Repositories;
using VaBank.Core.App;
using VaBank.Core.Maintenance;

namespace VaBank.Data.EntityFramework.App
{
    public class AuditLogRepository : IRepository, IAuditLogRepository
    {
        protected readonly DbContext Context;
        protected readonly IDatabaseProvider DatabaseProvider;
       
        public AuditLogRepository(DbContext context, IDatabaseProvider databaseProvider)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (databaseProvider == null)
                throw new ArgumentNullException("databaseProvider");
            DatabaseProvider = databaseProvider;
            Context = context;
        }

        public IList<AuditLogBriefEntry> Query(DbQuery<ApplicationAction> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() =>
            {
                var actions = Context.Set<ApplicationAction>().Include(x => x.Operation).Query(query).ToLookup(x => x.Operation).ToList();
                var entries = actions.Select(x => new AuditLogBriefEntry(x.Key, x));
                return entries.ToList();
            });
        }

        public AuditLogEntry GetAuditEntryDetails(Guid operationId)
        {
            var operation = Context.Set<Operation>().Find(operationId);
            if (operation == null)
                return null;

            return EnsureRepositoryException(() =>
            {
                var actions = Context.Set<ApplicationAction>().Where(x => x.Operation.Id == operation.Id).ToList();
                var auditTableNames = GetHistoryTableNames();
                var command = DatabaseProvider.CreateCommand();
                
                var sqlParameter = new SqlParameter("@OperationId", SqlDbType.UniqueIdentifier)
                {
                    Value = operation.Id
                };
                command.Parameters.Add(sqlParameter);

                var dbActions = new List<DatabaseAction>();
                var adapter = DatabaseProvider.CreateAdapter();
                adapter.SelectCommand = command;

                var constantNames = Names.GetColumnNames();

                foreach (var tableName in auditTableNames)
                {
                    command.CommandText = string.Format(
                        "SELECT * {0} WHERE HistoryOperationId = @OperationId",
                        tableName.GetFullTableName());
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    var dbAction = new DatabaseAction
                    {
                        TableName = tableName.GetFullTableName(),
                        ChangedRows = new List<VersionedDatabaseRow>()
                    };
                    dbActions.Add(dbAction);
                    
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var values = new Dictionary<string, object>();
                        var dbRow = new VersionedDatabaseRow(values);

                        dbAction.ChangedRows.Add(dbRow);

                        dbRow.Version = (long) row[Names.VersionColumnName];
                        dbRow.TimestampUtc = (DateTime) row[Names.TimestampUtcColumnName];
                        dbRow.Action = ToOperation(((char) row[Names.ActionColumnName]));

                        foreach (
                            var column in
                                dataTable.Columns.Cast<DataColumn>()
                                    .Where(column => !constantNames.Contains(column.ColumnName)))
                        {
                            values.Add(column.ColumnName, row[column.ColumnName]);
                        }
                    }
                }

                return new AuditLogEntry(operation, actions, dbActions);
            });
        }

        private IEnumerable<TableNamePair> GetHistoryTableNames()
        {
            const string sql =
                @"SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME LIKE '%[_]History'";

            var auditTableNames = Context.Database.SqlQuery<TableNamePair>(sql).ToList();
            return auditTableNames;
        }

        public IList<string> GetUniqueCodes()
        {
            return
                EnsureRepositoryException(
                    () => Context.Set<ApplicationAction>().Select(x => x.Code).Distinct().ToList());
        }

        public void CreateAction(ApplicationAction action)
        {
            EnsureRepositoryException(() => Context.Set<ApplicationAction>().Add(action));
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

        private class TableNamePair
        {
            public string TableSchema { get; set; }

            public string TableName { get; set; }

            public string GetFullTableName()
            {
                return string.Format("[{0}].[{1}]", TableSchema, TableName);
            }
        }

        private static class Names
        {
            public const string VersionColumnName = "HistoryId";
            public const string TimestampUtcColumnName = "HistoryTimestampUtc";
            public const string ActionColumnName = "HistoryAction";
            
            public static IList<string> GetColumnNames()
            {
                var type = typeof (Names);
                return type.GetFields(BindingFlags.Public | BindingFlags.Static |
                                      BindingFlags.FlattenHierarchy)
                    .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                    .Select(x => x.GetRawConstantValue())
                    .OfType<string>()
                    .ToList();
            }
        }
    }
}
