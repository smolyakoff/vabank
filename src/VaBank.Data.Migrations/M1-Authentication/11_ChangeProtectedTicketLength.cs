using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(11, "Changed protected ticket length in application token")]
    public class ChangeProtectedTicketLength : Migration
    {
        public override void Up()
        {
            Alter.Column("ProtectedTicket").OnTable("ApplicationToken").InSchema("Membership")
                .AsBigString();
        }

        public override void Down()
        {
            Alter.Column("ProtectedTicket").OnTable("ApplicationToken").InSchema("Membership")
                .AsSecurityString();
        }
    }
}
