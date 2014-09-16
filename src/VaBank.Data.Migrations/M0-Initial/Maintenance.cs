using FluentMigrator;

namespace VaBank.Data.Migrations.M0_Initial
{
    [Migration(1, "Maintenance tables")]
    [Profile("Maintenance")]
    public class Maintenance : Migration
    {
        public override void Up()
        {
            Create.Schema("Maintenance");
            Create.Table("Logs").InSchema("Maintenance")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_Logs")
                .WithColumn("Timestamp").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Logs");
            Delete.Schema("Maintenance");
        }
    }
}
