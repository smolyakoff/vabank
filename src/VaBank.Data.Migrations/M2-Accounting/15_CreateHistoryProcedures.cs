using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(15, "Create stored procedures for generating and dropping history tables")]
    [Tags("History", "Development", "Production", "Test")]
    public class CreateHistoryProcedures : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("M2_Accounting.PROC_GenerateHistoryTable.sql");
            Execute.EmbeddedScript("M2_Accounting.PROC_DropHistoryTable.sql");
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE [App].[GenerateHistoryTable], [App].[DropHistoryTable]");
        }
    }
}
