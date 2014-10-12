using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(4, "Add rowversion columns to User and UserProfile tables")]
    [Tags("Membership", "Development", "Production", "Test")]
    public class AddRowVersionColumns : Migration
    {
        public override void Up()
        {
            Create.Column("RowVersion")
                .OnTable("User").InSchema("Membership").AsCustom("timestamp").NotNullable();
            Create.Column("RowVersion")
                .OnTable("UserProfile").InSchema("Membership").AsCustom("timestamp").NotNullable();
        }

        public override void Down()
        {
            Delete.Column("RowVersion").FromTable("User").InSchema("Membership");
            Delete.Column("RowVersion").FromTable("UserProfile").InSchema("Membership");
        }
    }
}
