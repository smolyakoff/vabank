using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(18, "Tables for card accounts")]
    [Tags("Accounting", "Development", "Production", "Test")]
    public class Accounting: Migration
    {
        private const string SchemaName = "Accounting";
        private const string MembershipSchemaName = "Membership";

        public override void Down()
        {
            Delete.Table("User_Account").InSchema(SchemaName);
            Delete.Table("User_Card").InSchema(SchemaName);
            Delete.Table("Account_Card").InSchema(SchemaName);
            Delete.Table("CardSettings").InSchema(SchemaName);
            Delete.Table("Card").InSchema(SchemaName);
            Delete.Table("CardVendor").InSchema(SchemaName);
            Delete.Table("Account").InSchema(SchemaName);
            Delete.Table("Currency").InSchema(SchemaName);
        }

        public override void Up()
        {
            Create.Schema(SchemaName);

            Create.Table("Currency").InSchema(SchemaName)
                .WithColumn("CurrencyISOName").AsCurrencyISOName().PrimaryKey("PK_Currency")
                .WithColumn("Name").AsName().NotNullable()
                .WithColumn("Symbol").AsString(5).NotNullable();

            Create.Table("CardVendor").InSchema(SchemaName).WithColumn("ID").AsCardVendorId().PrimaryKey("PK_CardVendor");

            Create.Table("Account").InSchema(SchemaName)
                .WithColumn("AccountNo").AsAccountNumber().PrimaryKey("PK_Account")
                .WithColumn("UserID").AsGuid().NotNullable().ForeignKey("FK_Account_To_User", MembershipSchemaName, "User", "UserID")
                .WithColumn("CurrencyISOName").AsCurrencyISOName().ForeignKey("FK_Account_To_Currency", SchemaName, "Currency", "CurrencyISOName")
                .WithColumn("Balance").AsDecimal().NotNullable().WithDefaultValue(0)
                .WithColumn("OpenDateUtc").AsDateTime().NotNullable()
                .WithColumn("ExpirationDateUtc").AsDateTime().NotNullable()
                .WithColumn("Type").AsShortName().NotNullable();

            Create.Table("Card").InSchema(SchemaName)
                .WithColumn("CardID").AsCardId().PrimaryKey("PK_Card")
                .WithColumn("CardNo").AsCardNumber().NotNullable().Indexed("IX_Card_CardNo")
                .WithColumn("HolderFirstName").AsName().NotNullable()
                .WithColumn("HolderLastName").AsName().NotNullable()
                .WithColumn("ExpirationDateUtc").AsDateTime().NotNullable()
                .WithColumn("CardVendorID").AsCardVendorId().NotNullable().ForeignKey("FK_Card_To_CardVendor", SchemaName, "CardVendor", "ID");

            Create.Table("CardSettings").InSchema(SchemaName)
                .WithColumn("CardID").AsGuid().PrimaryKey("PK_CardSettings").ForeignKey("FK_CardSettings_To_Card", SchemaName, "Card", "CardID")
                .WithColumn("UserID").AsGuid().ForeignKey("FK_CardSettings_To_Card", MembershipSchemaName, "User", "UserID")
                .WithColumn("Blocked").AsBoolean().NotNullable()
                .WithColumn("BlockedDateUtc").AsDateTime()
                .WithColumn("FriendlyName").AsShortName()
                .WithColumn("LimitAmountPerDayLocal").AsDecimal().NotNullable()
                .WithColumn("LimitAmountPerDayAbroad").AsDecimal().NotNullable()
                .WithColumn("LimitOperationsPerDayLocal").AsDecimal().NotNullable()
                .WithColumn("LimitOperationsPerDayAbroad").AsDecimal().NotNullable();

            Create.Table("Account_Card").InSchema(SchemaName)
                .WithColumn("CardID").AsGuid().PrimaryKey("PK_Account_Card").ForeignKey("FK_Account_Card_To_Card", SchemaName, "Card", "CardID")
                .WithColumn("AccountNo").AsAccountNumber().PrimaryKey("PK_Account_Card").ForeignKey("FK_Account_Card_To_Account", SchemaName, "Account", "AccountNo");

            Create.Table("User_Card").InSchema(SchemaName)
                .WithColumn("UserId").AsGuid().PrimaryKey("PK_User_Card").ForeignKey("FK_User_Card_To_User", MembershipSchemaName, "User", "UserID")
                .WithColumn("CardID").AsGuid().PrimaryKey("PK_User_Card").ForeignKey("FK_User_Card_To_Card", SchemaName, "Card", "CardID");

            Create.Table("User_Account").InSchema(SchemaName)
                .WithColumn("UserId").AsGuid().PrimaryKey("PK_User_Account").ForeignKey("FK_User_Account_To_User", MembershipSchemaName, "User", "UserID")
                .WithColumn("AccountNo").AsAccountNumber().PrimaryKey("PK_User_Account").ForeignKey("FK_User_Account_To_Account", SchemaName, "Account", "AccountNo");
        }
    }
}
