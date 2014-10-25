using System;
using System.Collections.Generic;
using System.Linq;

namespace VaBank.Core.App.Entities
{
    public class DatabaseAction
    {
        public DatabaseAction(string tableName, IEnumerable<VersionedDatabaseRow> changedRows)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName");
            }
            if (changedRows == null)
            {
                throw new ArgumentNullException("changedRows");
            }
            var versionedDatabaseRows = changedRows as List<VersionedDatabaseRow> ?? changedRows.ToList();
            if (!versionedDatabaseRows.Any())
            {
                throw new ArgumentOutOfRangeException("changedRows", "The database action should contain at least one change");
            }
            TableName = tableName;
            Rows = versionedDatabaseRows;
        }

        public string TableName { get; private set; }

        public List<VersionedDatabaseRow> Rows { get; private set; } 
    }
}
