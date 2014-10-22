using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(21, "Create [Maintenance].[Setting] table.")]
    [Tags("Maintenance", "Development", "Production", "Test")]
    public class CreateMaintenanceSetting : Migration
    {
        public override void Down()
        {
            Delete.Table("Setting").InSchema("Maintenance");
        }

        public override void Up()
        {
            Create.Table("Setting").InSchema("Maintenance")
                .WithColumn("Key").AsName().PrimaryKey("PK_Setting")
                .WithColumn("Value").AsXml().Nullable();
        }
    }
}
