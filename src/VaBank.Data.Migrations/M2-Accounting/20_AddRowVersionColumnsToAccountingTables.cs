using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(20, "Add rowversion columns to Account table")]
    [Tags("Accounting", "Development", "Production", "Test")]
    public class AddRowVersionColumnsToAccountingTables: Migration
    {
        public override void Up()
        {
            Create.Column("RowVersion")
                .OnTable("Account").InSchema("Accounting").AsCustom("timestamp").NotNullable();
        }

        public override void Down()
        {
            Delete.Column("RowVersion").FromTable("Account").InSchema("Accounting");
        }
    }
}
