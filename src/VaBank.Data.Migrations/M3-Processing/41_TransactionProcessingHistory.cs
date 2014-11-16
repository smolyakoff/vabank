using FluentMigrator;

namespace VaBank.Data.Migrations.M3_Processing
{
    [Migration(41, "Generate history tables for transaction processing tables.")]
    [Tags("Development", "Production", "Test")]
    public class TransactionProcessingHistory : Migration
    {
        public override void Up()
        {
            Execute.Sql("EXEC [App].[GenerateHistoryTable] 'Transaction', 'Processing'");
            Execute.Sql("EXEC [App].[GenerateHistoryTable] 'Operation', 'Processing'");
        }

        public override void Down()
        {
            Execute.Sql("EXEC [App].[DropHistoryTable] 'Transaction', 'Processing'");
            Execute.Sql("EXEC [App].[DropHistoryTable] 'Operation', 'Processing'");
        }
    }
}
