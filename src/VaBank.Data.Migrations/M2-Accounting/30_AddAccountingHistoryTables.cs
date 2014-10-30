using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(30, "Add some history tables for audit purposes.")]
    [Tags("Development", "Test", "Production")]
    public class AddAccountingHistoryTables : Migration
    {
        public override void Down()
        {
            Execute.Sql("EXEC [App].[DropHistoryTable] 'Account', 'Accounting'");
            Execute.Sql("EXEC [App].[DropHistoryTable] 'CardSettings', 'Accounting'");
            Execute.Sql("EXEC [App].[DropHistoryTable] 'User_Card_Account', 'Accounting'");
        }

        public override void Up()
        {
            Execute.Sql("EXEC [App].[GenerateHistoryTable] 'Account', 'Accounting'");
            Execute.Sql("EXEC [App].[GenerateHistoryTable] 'CardSettings', 'Accounting'");
            Execute.Sql("EXEC [App].[GenerateHistoryTable] 'User_Card_Account', 'Accounting'");
        }
    }
}
