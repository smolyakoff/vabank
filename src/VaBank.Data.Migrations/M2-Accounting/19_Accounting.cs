using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(19, "Tables for card accounts")]
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

            Create.Table("CardVendor").InSchema(SchemaName)
                .WithColumn("ID").AsCardVendorId().PrimaryKey("PK_CardVendor")
                .WithColumn("Name").AsShortName().NotNullable();

            Create.Table("Account").InSchema(SchemaName)
                .WithColumn("AccountNo").AsAccountNumber().PrimaryKey("PK_Account")
                .WithColumn("CurrencyISOName").AsCurrencyISOName().ForeignKey("FK_Account_To_Currency", SchemaName, "Currency", "CurrencyISOName")
                .WithColumn("Balance").AsDecimal().NotNullable().WithDefaultValue(0)
                .WithColumn("OpenDateUtc").AsDate().NotNullable()
                .WithColumn("ExpirationDateUtc").AsDate().NotNullable()
                .WithColumn("Type").AsShortName().NotNullable();

            Create.Table("Card").InSchema(SchemaName)
                .WithColumn("CardID").AsCardId().PrimaryKey("PK_Card")
                .WithColumn("CardNo").AsCardNumber().NotNullable().Unique("IX_Card_CardNo")
                .WithColumn("HolderFirstName").AsName().NotNullable()
                .WithColumn("HolderLastName").AsName().NotNullable()
                .WithColumn("ExpirationDateUtc").AsDate().NotNullable()
                .WithColumn("CardVendorID").AsCardVendorId().NotNullable().ForeignKey("FK_Card_To_CardVendor", SchemaName, "CardVendor", "ID");

            Create.Table("User_Account").InSchema(SchemaName)
                .WithColumn("UserID").AsGuid().NotNullable().ForeignKey("FK_User_Account_To_User", MembershipSchemaName, "User", "UserID")
                .WithColumn("AccountNo").AsAccountNumber().PrimaryKey("PK_User_Account").ForeignKey("FK_User_Account_To_Account", SchemaName, "Account", "AccountNo");

            Create.Table("User_Card_Account").InSchema(SchemaName)
                .WithColumn("UserID").AsUserId().NotNullable()
                    .ForeignKey("FK_User_Card_Account_To_User", MembershipSchemaName, "User", "UserID")
                .WithColumn("CardID").AsCardId().NotNullable()
                    .ForeignKey("FK_User_Card_Account_To_Card", SchemaName, "Card", "CardID")
                    .PrimaryKey("PK_User_Card_Account")
                .WithColumn("AccountNo").AsAccountNumber().Nullable()
                    .ForeignKey("FK_User_Card_Account_To_User_Account", SchemaName, "User_Account", "AccountNo");

            Create.Table("CardSettings").InSchema(SchemaName)
                .WithColumn("CardID").AsGuid().PrimaryKey("PK_CardSettings").ForeignKey("FK_CardSettings_To_User_Card_Account", SchemaName, "User_Card_Account", "CardID")
                .WithColumn("Blocked").AsBoolean().NotNullable()
                .WithColumn("BlockedDateUtc").AsDateTime().Nullable()
                .WithColumn("FriendlyName").AsShortName().Nullable()
                .WithColumn("LimitAmountPerDayLocal").AsDecimal().NotNullable()
                .WithColumn("LimitAmountPerDayAbroad").AsDecimal().NotNullable()
                .WithColumn("LimitOperationsPerDayLocal").AsInt32().NotNullable()
                .WithColumn("LimitOperationsPerDayAbroad").AsInt32().NotNullable();
        }
    }
}
