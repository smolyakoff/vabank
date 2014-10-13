using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(14, "Created app schema and OperationMarker")]
    public class CreateAppOperationMarker : Migration
    {
        private const string SchemaName = "App";

        public override void Up()
        {
            Create.Schema(SchemaName);
            Execute.EmbeddedScript("M2_Accounting.TABLE_OperationMarker.sql");
            Execute.EmbeddedScript("M2_Accounting.TRG_OperationMarker_Insert.sql");
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
                .OnColumn("TransactionId").Ascending()
                .OnColumn("TimestampUtc").Descending();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_OperationMarker_To_User");
            Delete.ForeignKey("FK_OperationMarker_To_ApplicationClient");
            Delete.Table("OperationMarker").InSchema("App");
            Delete.Schema(SchemaName);
        }
    }
}
