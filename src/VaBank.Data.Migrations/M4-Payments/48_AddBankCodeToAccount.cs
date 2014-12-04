using FluentMigrator;
using VaBank.Data.Migrations;

namespace VaBank.Data.Migrations
{
    [Migration(48, "Add bank code column to [Accounting].[Account] table.")]
    [Tags("Development", "Test", "Production")]
    public class AddBankCodeToAccount: Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter.Table("Account").InSchema("Accounting").AddColumn("BankCode").AsString(9).ForeignKey("FK_Account_To_Bank", "Payments", "Bank", "Code").Nullable();
            Update.Table("Account").InSchema("Accounting").Set(new { BankCode = "153001966" }).AllRows();
            Alter.Table("Account").InSchema("Accounting").AlterColumn("BankCode").AsString(9).NotNullable();
        }
    }
}
