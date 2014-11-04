using FluentMigrator;

namespace VaBank.Data.Migrations.M3_Processing
{
    //[Migration(33, "Added security code table to store sms security codes.")]
    [Tags("Development", "Test", "Production")]
    public class AddSecurityCodeTable : Migration
    {
        public override void Up()
        {
            //empty
        }

        public override void Down()
        {
            //empty
        }
    }
}
