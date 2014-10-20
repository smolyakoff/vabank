using System.Collections.Generic;

namespace VaBank.Core.App
{
    public class DatabaseAction
    {
        public string TableName { get; set; }

        public List<VersionedDatabaseRow> ChangedRows { get; set; } 

    }
}
