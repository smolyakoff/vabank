using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(1, "Table for logging purposes")]
    [Tags("Maintenance", "Development", "Production","Test")]
    public class Logging : Migration
    {
        public override void Up()
        {
            Create.Table("Logs").InSchema("Maintenance")
                .WithColumn("EventId").AsInt64().PrimaryKey("PK_Logs").Identity()
                .WithColumn("Application").AsString(50).NotNullable().WithDefaultValue("VaBank")
                .WithColumn("TimestampUtc").AsDateTime().NotNullable().Indexed("IX_Logs_TimestampUtc").WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("Level").AsString(20).NotNullable()
                .WithColumn("Type").AsString(50).NotNullable().WithDefaultValue("Generic").Indexed("IX_Logs_Type")
                .WithColumn("User").AsString(100).Nullable().Indexed("IX_Logs_User")
                .WithColumn("Message").AsString().NotNullable()
                .WithColumn("Exception").AsString().Nullable()
                .WithColumn("Source").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Logs");
        }
    }
}
