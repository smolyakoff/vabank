using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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
            {
                return null;
            }
            return EnsureRepositoryException(() =>
            {
                var actions = Context.Set<ApplicationAction>().Where(x => x.Operation.Id == operation.Id).ToList();
                var dbActions = GetHistoryTableNames()
                    .Select(x => new { dataTable = GetHistory(x, operationId), tableName = x})
                    .Where(x => x.dataTable.Rows.Count > 0)
                    .Select(x => new { x.tableName, rows = x.dataTable.Rows.Cast<DataRow>().Select(ToVersionedRow).ToList()})
                    .Select(x => new DatabaseAction(x.tableName.FullName, x.rows))
                    .ToList();
                return new AuditLogEntry(operation, actions, dbActions);
            });
        }

        public IList<string> GetUniqueCodes()
        {
            return EnsureRepositoryException(() => Context.Set<ApplicationAction>().Select(x => x.Code).Distinct().ToList());
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

        private DataTable GetHistory(TableName tableName, Guid operationId)
        {
            
            var adapter = DatabaseProvider.CreateAdapter();
            adapter.SelectCommand = DatabaseProvider.CreateCommand();
            var sqlOperationId = adapter.SelectCommand.CreateParameter();
            sqlOperationId.ParameterName = "@OperationId";
            sqlOperationId.DbType = DbType.Guid;
            sqlOperationId.Value = operationId;
            adapter.SelectCommand.CommandText = 
                string.Format("SELECT * FROM {0} WHERE HistoryOperationId = @OperationId", tableName.FullName);
            adapter.SelectCommand.Parameters.Add(sqlOperationId);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        private IEnumerable<TableName> GetHistoryTableNames()
        {
            const string sql =
                @"SELECT TABLE_SCHEMA as [Schema], TABLE_NAME as [Name] FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME LIKE '%[_]History'";

            var auditTableNames = Context.Database.SqlQuery<TableName>(sql).ToList();
            return auditTableNames;
        }

        private static VersionedDatabaseRow ToVersionedRow(DataRow dataRow)
        {
            var constantNames = Names.GetColumnNames();

            var version = (long)dataRow[Names.VersionColumnName];
            var timestampUtc = (DateTime)dataRow[Names.TimestampUtcColumnName];
            var action = ToOperation((char)dataRow[Names.ActionColumnName]);

            var otherColumns = dataRow.Table.Columns
                .Cast<DataColumn>()
                .Where(x => !constantNames.Contains(x.ColumnName));

            var row = new VersionedDatabaseRow(version, timestampUtc, action);

            foreach (var column in otherColumns)
            {
                row.SetValue(column.ColumnName, dataRow[column]);
            }
            return row;
        }
        
        private static DatabaseOperation ToOperation(char value)
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

        private class TableName
        {
            public string Schema { get; set; }

            public string Name { get; set; }

            public string FullName
            {
                get { return string.Format("[{0}].[{1}]", Schema, Name); }
            }
        }

        private static class Names
        {
            public const string VersionColumnName = "HistoryId";
            public const string TimestampUtcColumnName = "HistoryTimestampUtc";
            public const string ActionColumnName = "HistoryAction";
            
            //TODO: make this code usable for any type: move to ConstantCollection base class in VaBank.Common.Util namespace?
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
