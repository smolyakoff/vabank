using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(14, "Created current transaction id proc for sql azure.")]
    [Tags("Production")]
    public class CreateCurrentTransactionIdProd : Migration
    {
        public override void Up()
        {
            Create.Schema("App");
            Execute.EmbeddedScript("M2_Accounting.PROC_CurrentTransactionId_Azure.sql");
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE [App].[CurrentTransactionId]");
            Delete.Schema("App");
        }
    }
}
