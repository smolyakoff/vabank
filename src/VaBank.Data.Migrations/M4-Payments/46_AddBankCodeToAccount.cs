using FluentMigrator;
using VaBank.Data.Migrations;

namespace VaBank.Data.Migrations
{
    [Migration(46, "Add bank code column to [Accounting].[Account] table.")]
    [Tags("Development", "Test", "Production")]
    public class AddBankCodeToAccount: Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter.Table("Account").InSchema("Accounting").AddColumn("BankCode").AsString(9).NotNullable();
            Update.Table("Account").InSchema("Accounting").Set(new { BankCode = "153001966" }).AllRows();
        }
    }
}
