using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(13, "Chage PK for UserClaim table")]
    public class ChangeUserClaimPK : Migration
    {

        public override void Down()
        {
            //Do nothing
        }

        public override void Up()
        {
            Delete.PrimaryKey("PK_UserClaim").FromTable("UserClaim").InSchema("Membership");
            Create.PrimaryKey("PK_UserClaim").OnTable("UserClaim").WithSchema("Membership")
                .Columns("UserID", "Type", "Value");
        }
    }
}
