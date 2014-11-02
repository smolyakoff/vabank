using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(15, "Created app schema and Operation")]
    [Tags("App", "Development", "Production", "Test")]
    public class CreateAppOperation : Migration
    {
        private const string SchemaName = "App";

        public override void Up()
        {
            Execute.EmbeddedScript("M2_Accounting.TABLE_Operation.sql");
            Execute.EmbeddedScript("M2_Accounting.TRG_Operation_Insert.sql");
            Execute.EmbeddedScript("M2_Accounting.PROC_StartOperation.sql");
            Execute.EmbeddedScript("M2_Accounting.PROC_CurrentOperationId.sql");
            Execute.EmbeddedScript("M2_Accounting.PROC_FinishOperation.sql");

            Create.ForeignKey("FK_Operation_To_User")
                .FromTable("Operation").InSchema(SchemaName).ForeignColumn("AppUserID")
                .ToTable("User").InSchema("Membership").PrimaryColumn("UserID");
            Create.ForeignKey("FK_Operation_To_ApplicationClient")
                .FromTable("Operation").InSchema(SchemaName).ForeignColumn("AppClientID")
                .ToTable("ApplicationClient").InSchema("Membership").PrimaryColumn("ID");

            Create.Index("IX_Operation_StartedUtc")
                .OnTable("Operation").InSchema("App")
                .OnColumn("StartedUtc").Descending();

            Create.Index("IX_Operation_AK")
                .OnTable("Operation").InSchema("App")
                .OnColumn("TransactionID").Descending()
                .OnColumn("Finished").Ascending()
                .OnColumn("StartedUtc").Descending();
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE [App].[StartOperation], [App].[FinishOperation]");
            Execute.Sql("DROP PROCEDURE [App].[CurrentOperationId]");
            Delete.ForeignKey("FK_Operation_To_User");
            Delete.ForeignKey("FK_Operation_To_ApplicationClient");
            Delete.Table("Operation").InSchema("App");
            
            Delete.Schema(SchemaName);
        }
    }
}
