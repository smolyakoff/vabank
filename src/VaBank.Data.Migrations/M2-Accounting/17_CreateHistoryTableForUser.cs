using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(17, "Added history table for [Membership].[User]")]
    [Tags("History", "Development", "Production", "Test")]
    public class CreateHistoryTableForUser : Migration
    {
        public override void Up()
        {
            Execute.Sql("EXEC [App].[GenerateHistoryTable] 'User', 'Membership'");
        }

        public override void Down()
        {
            Execute.Sql("EXEC [App].[DropHistoryTable] 'User', 'Membership'");
        }
    }
}