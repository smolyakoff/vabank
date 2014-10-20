using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(19, "Create resource table to store application resources")]
    [Tags("Resource", "Development", "Production", "Test")]
    public class CreateResource : Migration
    {
        private const string SchemaName = "App";

        public override void Up()
        {
            Create.Table("Resource").InSchema(SchemaName)
                .WithColumn("ID").AsGuid().PrimaryKey("PK_Resource")
                .WithColumn("Type").AsShortName().NotNullable().WithDefaultValue("BlobDatabase")
                .WithColumn("Location").AsName().NotNullable().WithDefaultValue("Database")
                .WithColumn("Uri").AsBigString().Nullable()
                .WithColumn("Data").AsBinary().Nullable();

            Create.Index("IX_Resource_Uri").OnTable("Resource").InSchema(SchemaName).OnColumn("Uri");
        }

        public override void Down()
        {
            Delete.Table("Resource");
        }
    }
}
