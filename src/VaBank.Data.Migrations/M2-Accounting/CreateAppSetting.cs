using FluentMigrator;

namespace VaBank.Data.Migrations.M2_Accounting
{
    [Migration(21, "Create [App].[Setting] table.")]
    [Tags("Maintenance", "Development", "Production", "Test")]
    public class CreateAppSetting : Migration
    {
        public override void Down()
        {
            Delete.Table("Setting").InSchema("App");
        }

        public override void Up()
        {
            Create.Table("Setting").InSchema("App")
                .WithColumn("Key").AsName().PrimaryKey("PK_Setting")
                .WithColumn("Value").AsXml().Nullable();
        }
    }
}
