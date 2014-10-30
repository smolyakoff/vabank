using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(23, "Filling empty settings for tests.")]
    [Tags("Maintenance", "Test")]
    public class SeedTestSettings : Migration
    {
        public override void Down()
        {
            Delete.FromTable("Setting").InSchema("App").Row(new { Key = "Test_Man" });
        }

        public override void Up()
        {            
            Insert.IntoTable("Setting").InSchema("App").Row(new { Key = "Test_Man", Value = (string)null });
        }
    }
}
