using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(14, "Created current transaction id proc for sql server.")]
    [Tags("Development", "Test")]
    public class CreateCurrentTransactionIdDev : Migration
    {
        public override void Up()
        {
            Create.Schema("App");
            Execute.EmbeddedScript("M2_Accounting.PROC_CurrentTransactionId.sql");
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE [App].[CurrentTransactionId]");
            Delete.Schema("App");
        }
    }
}
