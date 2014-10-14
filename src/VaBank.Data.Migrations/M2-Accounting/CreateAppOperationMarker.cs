using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(14, "Created app schema and OperationMarker")]
    [Tags("App", "Development", "Production", "Test")]
    public class CreateAppOperationMarker : Migration
    {
        private const string SchemaName = "App";

        public override void Up()
        {
            Create.Schema(SchemaName);
            Execute.EmbeddedScript("M2_Accounting.TABLE_OperationMarker.sql");
            Execute.EmbeddedScript("M2_Accounting.TRG_OperationMarker_Insert.sql");
            Execute.EmbeddedScript("M2_Accounting.PROC_StartOperationMarker.sql");
            Execute.EmbeddedScript("M2_Accounting.FUNC_GetOperationMarker.sql");
            Execute.EmbeddedScript("M2_Accounting.PROC_FinishOperationMarker.sql");

            Create.ForeignKey("FK_OperationMarker_To_User")
                .FromTable("OperationMarker").InSchema(SchemaName).ForeignColumn("AppUserId")
                .ToTable("User").InSchema("Membership").PrimaryColumn("UserId");
            Create.ForeignKey("FK_OperationMarker_To_ApplicationClient")
                .FromTable("OperationMarker").InSchema(SchemaName).ForeignColumn("AppClientId")
                .ToTable("ApplicationClient").InSchema("Membership").PrimaryColumn("ID");

            Create.Index("IX_OperationMarker_TimestampUtc")
                .OnTable("OperationMarker").InSchema("App")
                .OnColumn("TimestampUtc").Descending();

            Create.Index("IX_OperationMarker_AK")
                .OnTable("OperationMarker").InSchema("App")
                .OnColumn("TransactionId").Descending()
                .OnColumn("Finished").Ascending()
                .OnColumn("TimestampUtc").Descending();
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE [App].[StartOperationMarker], [App].[FinishOperationMarker]");
            Execute.Sql("DROP FUNCTION [App].[GetOperationMarker]");
            Delete.ForeignKey("FK_OperationMarker_To_User");
            Delete.ForeignKey("FK_OperationMarker_To_ApplicationClient");
            Delete.Table("OperationMarker").InSchema("App");
            
            Delete.Schema(SchemaName);
        }
    }
}
