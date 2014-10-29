using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(10, "Add user id column to ApplicationTokenClient table")]
    public class CreateApplicationTokenUserIdColumn : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("FK_ApplicationToken_To_User").OnTable("ApplicationToken").InSchema("Membership");
            Delete.Column("UserId").FromTable("ApplicationToken").InSchema("Membership");
        }

        public override void Up()
        {
            Create.Column("UserId").OnTable("ApplicationToken").InSchema("Membership")
                .AsGuid().NotNullable();
            Create.ForeignKey("FK_ApplicationToken_To_User").FromTable("ApplicationToken").InSchema("Membership")
                .ForeignColumn("UserId").ToTable("User").InSchema("Membership").PrimaryColumn("UserId");
        }
    }
}
