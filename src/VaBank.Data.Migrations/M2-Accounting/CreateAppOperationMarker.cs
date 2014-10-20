using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(14, "Created app schema and Operation")]
    [Tags("App", "Development", "Production", "Test")]
    public class CreateAppOperation : Migration
    {
        private const string SchemaName = "App";

        public override void Up()
        {
            Create.Schema(SchemaName);
            Execute.EmbeddedScript("M2_Accounting.TABLE_Operation.sql");
            Execute.EmbeddedScript("M2_Accounting.TRG_Operation_Insert.sql");
            Execute.EmbeddedScript("M2_Accounting.PROC_StartOperation.sql");
            Execute.EmbeddedScript("M2_Accounting.VIEW_CurrentOperation.sql");
            Execute.EmbeddedScript("M2_Accounting.PROC_FinishOperation.sql");

            Create.ForeignKey("FK_Operation_To_User")
                .FromTable("Operation").InSchema(SchemaName).ForeignColumn("AppUserId")
                .ToTable("User").InSchema("Membership").PrimaryColumn("UserId");
            Create.ForeignKey("FK_Operation_To_ApplicationClient")
                .FromTable("Operation").InSchema(SchemaName).ForeignColumn("AppClientId")
                .ToTable("ApplicationClient").InSchema("Membership").PrimaryColumn("ID");

            Create.Index("IX_Operation_TimestampUtc")
                .OnTable("Operation").InSchema("App")
                .OnColumn("TimestampUtc").Descending();

            Create.Index("IX_Operation_AK")
                .OnTable("Operation").InSchema("App")
                .OnColumn("TransactionId").Descending()
                .OnColumn("Finished").Ascending()
                .OnColumn("TimestampUtc").Descending();
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE [App].[StartOperation], [App].[FinishOperation]");
            Execute.Sql("DROP VIEW [App].[CurrentOperation]");
            Delete.ForeignKey("FK_Operation_To_User");
            Delete.ForeignKey("FK_Operation_To_ApplicationClient");
            Delete.Table("Operation").InSchema("App");
            
            Delete.Schema(SchemaName);
        }
    }
}
