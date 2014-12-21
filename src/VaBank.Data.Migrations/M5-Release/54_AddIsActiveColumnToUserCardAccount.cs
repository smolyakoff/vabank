using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(54, "Add IsActive column to User_Card_Account")]
    [Tags("Development", "Test", "Production")]
    public class AddIsActiveColumnToUserCardAccount : Migration
    {
        public override void Up()
        {
            Alter.Table("User_Card_Account").InSchema("Accounting")
                .AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

            //WARNING! All cards should be bound to accounts
            Alter.Column("AccountNo").OnTable("User_Card_Account").InSchema("Accounting")
                .AsAccountNumber()
                .NotNullable();
        }

        public override void Down()
        {
            Delete.Column("IsActive").FromTable("User_Card_Account").InSchema("Accounting");
            Alter.Column("AccountNo").OnTable("User_Card_Account").InSchema("Accounting")
                .AsAccountNumber()
                .Nullable();
        }
    }
}
