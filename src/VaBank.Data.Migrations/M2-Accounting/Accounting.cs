using FluentMigrator;

namespace VaBank.Data.Migrations.M2_Accounting
{
    [Migration(1, "Tables for card accounts")]
    [Tags("Accounting", "Development", "Test")]
    public class Accounting: Migration
    {
        private const string SchemaName = "Accounting";
        private const string MembershipSchemaName = "Membership";

        public override void Down()
        {
            throw new System.NotImplementedException();
        }

        public override void Up()
        {
            Create.Schema(SchemaName);

            Create.Table("Currency").InSchema(SchemaName).WithColumn("CurrencyISOName").AsCurrencyISOName().PrimaryKey("PK_Currency")
                .WithColumn("Name").AsName().NotNullable()
                .WithColumn("Symbol").AsString(1).NotNullable();

            Create.Table("CardVendor").InSchema(SchemaName).WithColumn("ID").AsCardVendorId().PrimaryKey("PK_CardVendor");

            Create.Table("Account").InSchema(SchemaName).WithColumn("AccountNo").AsAccountNumber().PrimaryKey("PK_Account")
                .WithColumn("UserID").AsGuid().NotNullable().ForeignKey("FK_Account_To_User", MembershipSchemaName, "User", "UserID")
                .WithColumn("CurrencyId").AsCurrencyISOName().ForeignKey("FK_Account_To_Currency", SchemaName, "Currency", "CurrencyISOName")
                .WithColumn("Balance").AsDecimal().NotNullable().WithDefaultValue(0)
                .WithColumn("OpenDateUtc").AsDateTime().NotNullable()
                .WithColumn("ExpirationDateUtc").AsDateTime().NotNullable();

            Create.Table("Card").InSchema(SchemaName).WithColumn("CardID").AsCardId().PrimaryKey("PK_Card")
                .WithColumn("CardNo").AsCardNumber().NotNullable()
                .WithColumn("HolderFirstName").AsName().NotNullable()
                .WithColumn("HolderLastName").AsName().NotNullable()
                .WithColumn("ExpirationDateUtc").AsDateTime().NotNullable()
                .WithColumn("ExpirationDate").AsDateTime().NotNullable()
                .WithColumn("CardVendorID").AsCardVendorId().NotNullable().ForeignKey("FK_Card_To_CardVendor", SchemaName, "CardVendor", "ID");

            Create.Table("CardSettings").InSchema(SchemaName)
                .WithColumn("CardID").AsGuid().PrimaryKey("PK_CardSettings").ForeignKey("FK_CardSettings_To_Card", SchemaName, "Card", "CardID")
                .WithColumn("UserID").AsGuid().ForeignKey("FK_CardSettings_To_Card", MembershipSchemaName, "User", "UserID")
                .WithColumn("Blocked").AsBoolean().NotNullable()
                .WithColumn("BlockedDateUtc").AsDateTime()
                .WithColumn("FriendlyName").AsShortName()
                .WithColumn("LimitAmountLocal").AsDecimal().NotNullable()
                .WithColumn("LimitAmountAbroad").AsDecimal().NotNullable();

            Create.Table("Account_Card").InSchema(SchemaName)
                .WithColumn("CardID").AsGuid().PrimaryKey("PK_Account_Card").ForeignKey("FK_Account_Card_To_Card", SchemaName, "Card", "CardID")
                .WithColumn("AccountNo").AsGuid().PrimaryKey("PK_Account_Card").ForeignKey("FK_Account_Card_To_Account", SchemaName, "Account", "AccountNo");

            Create.Table("User_Card").InSchema(SchemaName)
                .WithColumn("UserId").AsGuid().PrimaryKey("PK_User_Card").ForeignKey("FK_User_Card_To_User", MembershipSchemaName, "User", "UserID")
                .WithColumn("CardID").AsGuid().PrimaryKey("PK_User_Card").ForeignKey("FK_User_Card_To_Card", SchemaName, "Card", "CardID");
        }
    }
}
