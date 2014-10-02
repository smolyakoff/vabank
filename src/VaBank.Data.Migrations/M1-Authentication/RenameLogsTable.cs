using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(3, "Renamed Logs table to SystemLog")]
    [Tags("Maintenance", "Development", "Production")]
    public class RenameLogsTable : Migration
    {
        public override void Up()
        {
            Rename.Table("Logs").InSchema("Maintenance").To("SystemLog");
        }

        public override void Down()
        {
            Rename.Table("SystemLog").InSchema("Maintenance").To("Logs");
        }
    }
}
