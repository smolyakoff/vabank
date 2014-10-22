using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(22, "Filling empty settings.")]
    [Tags("Maintenance", "Development", "Test")]
    public class SeedTestSettings : Migration
    {
        public override void Down()
        {
            Delete.FromTable("Setting").InSchema("Maintenance").Row(new { Key = "Test_Man" });
        }

        public override void Up()
        {            
            Insert.IntoTable("Setting").InSchema("Maintenance").Row(new { Key = "Test_Man", Value = (string)null });
        }
    }
}
