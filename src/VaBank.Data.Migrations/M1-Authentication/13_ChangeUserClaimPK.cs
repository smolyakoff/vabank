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
            Execute.EmbeddedScript("M1_Authentication.ChangeUserClaimPK.sql");
        }
    }
}
