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
            get { return "DatabaseVersions"; }
        }

        public string UniqueIndexName
        {
            get { return "UC_DatabaseVersions_Version"; }
        }
    }
}