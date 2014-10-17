using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(17, "Create [App].[Action] table.")]
    [Tags("App", "Development", "Production", "Test")]
    public class CreateAppAction : Migration
    {
        public override void Up()
        {
            Create.Table("Action").InSchema("App")
                .WithColumn("EventId").AsGuid().PrimaryKey("PK_Action")
                .WithColumn("OperationId").AsGuid().NotNullable().ForeignKey("FK_Action_To_Operation", "App", "Operation", "Id")
                .WithColumn("Code").AsShortName().NotNullable().Indexed("IX_Action_Code").WithDefaultValue("OPERATION")
                .WithColumn("Description").AsBigString().Nullable()
                .WithColumn("Data").AsText().Nullable()
                .WithColumn("TimestampUtc").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Action").InSchema("App");
        }
    }
}
