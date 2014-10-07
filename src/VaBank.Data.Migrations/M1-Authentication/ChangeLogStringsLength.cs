using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(9, "Change message and exception columns length at SystemLog table")]
    [Tags("Maintenance", "Development", "Production", "Test")]
    public class ChangeLogStringsLength : Migration
    {
        public override void Up()
        {
            Alter.Column("Message").OnTable("SystemLog").InSchema("Maintenance").AsString(512);
            Alter.Column("Exception").OnTable("SystemLog").InSchema("Maintenance").AsString(2048);
        }

        public override void Down()
        {
            Alter.Column("Message").OnTable("SystemLog").InSchema("Maintenance").AsString();
            Alter.Column("Exception").OnTable("SystemLog").InSchema("Maintenance").AsString();
        }
    }
}
