using FluentMigrator.VersionTableInfo;

namespace VaBank.Data.Migrations
{
    [VersionTableMetaData]
    public class VersionTable : IVersionTableMetaData
    {
        public string ColumnName
        {
            get { return "Version"; }
        }

        public string DescriptionColumnName
        {
            get { return "Description"; }
        }

        public string SchemaName
        {
            get { return "Maintenance"; }
        }

        public string TableName
        {
            get { return "DbVersions"; }
        }

        public string UniqueIndexName
        {
            get { return "UC_DbVersions_Version"; }
        }
    }
}