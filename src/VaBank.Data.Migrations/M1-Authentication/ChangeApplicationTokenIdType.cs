using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(6, "Change ApplicationToken table Id type to string")]
    [Tags("Membership", "Development", "Production", "Test")]
    public class ChangeApplicationTokenIdType : Migration
    {
        public override void Up()
        {
            Delete.PrimaryKey("PK_ApplicationToken").FromTable("ApplicationToken").InSchema("Membership");
            Delete.Column("ID").FromTable("ApplicationToken").InSchema("Membership");

            Create.Column("ID").OnTable("ApplicationToken").InSchema("Membership").AsString(256).NotNullable();
            Create.PrimaryKey("PK_ApplicationToken").OnTable("ApplicationToken").WithSchema("Membership").Column("ID");
        }

        public override void Down()
        {
            Delete.PrimaryKey("PK_ApplicationToken").FromTable("ApplicationToken").InSchema("Membership");
            Delete.Column("ID").FromTable("ApplicationToken").InSchema("Membership");

            Create.Column("ID").OnTable("ApplicationToken").InSchema("Membership").AsGuid();
            Create.PrimaryKey("PK_ApplicationToken").OnTable("ApplicationToken").WithSchema("Membership").Column("ID");
        }
    }
}
