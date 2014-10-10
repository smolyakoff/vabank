using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(12, "Delete SecurityStamp column from User table")]
    public class DeleteSecurityStampFromUser : Migration
    {
        public override void Down()
        {
            Create.Column("SecurityStamp").OnTable("User").InSchema("Membership").AsSecurityString().NotNullable();
        }

        public override void Up()
        {
            Delete.Column("SecurityStamp").FromTable("User").InSchema("Membership");
        }
    }
}
