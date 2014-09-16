using FluentMigrator;

namespace VaBank.Data.Migrations.M0_Initial
{
    [Migration(1, "Maintenance tables")]
    [Tags("Maintenance")]
    public class Maintenance : Migration
    {
        public override void Up()
        {
            Create.Schema("Maintenance");
            Create.Table("Logs").InSchema("Maintenance")
                .WithColumn("ErrorId").AsInt64().PrimaryKey("PK_Logs").Identity()
                .WithColumn("Application").AsString(50).NotNullable().WithDefaultValue("VaBank")
                .WithColumn("TimestampUtc").AsDateTime().NotNullable().Indexed("IX_Logs_TimestampUtc").WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("Message").AsString().NotNullable()
                .WithColumn("Exception").AsString().Nullable()
                .WithColumn("Source").AsString().NotNullable()
                .WithColumn("Level").AsString(20).NotNullable()
                .WithColumn("Type").AsString(50).NotNullable().WithDefaultValue("Generic").Indexed("IX_Logs_Type")
                .WithColumn("User").AsString(100).Nullable().Indexed("IX_Logs_User");
        }

        public override void Down()
        {
            Delete.Table("Logs");
            Delete.Schema("Maintenance");
        }
    }
}
