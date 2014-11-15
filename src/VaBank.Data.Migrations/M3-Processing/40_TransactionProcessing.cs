using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(40, "Add transaction processing tables.")]
    [Tags("Development", "Production", "Test")]
    public class TransactionProcessing : Migration
    {
        private const string SchemaName = "Processing";

        public override void Up()
        {
            Create.Table("OperationCategory").InSchema(SchemaName)
                .WithColumn("Code").AsShortName().PrimaryKey("PK_OperationCategory")
                .WithColumn("Name").AsShortName().NotNullable()
                .WithColumn("Description").AsBigString().Nullable();

            Create.Table("Operation").InSchema(SchemaName)
                .WithColumn("ID").AsInt64().PrimaryKey("PK_Operation")
                .WithColumn("UserID")
                    .AsUserId()
                    .ForeignKey("FK_Operation_To_User", "Membership", "User", "UserID")
                    .NotNullable()
                .WithColumn("CategoryCode")
                    .AsShortName()
                .ForeignKey("FK_OperationCategory_To_Operation", SchemaName, "OperationCategory", "Code")
                    .NotNullable()
                .WithColumn("CreatedDateUtc").AsDateTime().NotNullable()
                .WithColumn("Status").AsInt32().NotNullable().Indexed("IX_Operation_Status");

            Create.Table("Transaction").InSchema(SchemaName)
                .WithColumn("ID").AsGuid().PrimaryKey("PK_Transaction")
                .WithColumn("AccountNo")
                    .AsAccountNumber()
                    .ForeignKey("FK_Account_To_Transaction", "Accounting", "Account", "AccountNo")
                    .NotNullable()
                    .Indexed("IX_Transaction_AccountNo")
                .WithColumn("CurrencyISOName")
                    .AsCurrencyISOName()
                    .ForeignKey("FK_Currency_To_Transaction", "Accounting", "Currency", "CurrencyISOName")
                    .NotNullable()
                .WithColumn("TransactionAmount").AsDecimal().NotNullable()
                .WithColumn("AccountAmount").AsDecimal().NotNullable()
                .WithColumn("RemainingBalance").AsDecimal().NotNullable()
                .WithColumn("CreatedDateUtc").AsDateTime().NotNullable()
                .WithColumn("PostDateUtc").AsDateTime().NotNullable()
                .WithColumn("Place").AsName().NotNullable().Indexed("IX_Transaction_Place")
                .WithColumn("Description").AsBigString().Nullable()
                .WithColumn("Status").AsInt32().NotNullable().Indexed("IX_Transaction_Status");

            Create.Table("Transfer").InSchema(SchemaName)
                .WithColumn("OperationID")
                    .AsInt64()
                    .PrimaryKey("PK_Transfer")
                    .ForeignKey("FK_Operation_To_Transfer", SchemaName, "Operation", "ID")
                .WithColumn("FromTransactionID")
                    .AsGuid()
                    .ForeignKey("FK_Transaction_To_Transfer_From", SchemaName, "Transaction", "ID")
                    .NotNullable()
                .WithColumn("ToTransactionID")
                    .AsGuid()
                    .ForeignKey("FK_Transaction_To_Transfer_To", SchemaName, "Transaction", "ID")
                    .NotNullable();

            Create.Table("CardTransfer").InSchema(SchemaName)
                .WithColumn("OperationID")
                    .AsInt64()
                    .PrimaryKey("PK_CardTransfer")
                    .ForeignKey("FK_Transfer_To_CardTransfer", SchemaName, "Transfer", "OperationID")
                .WithColumn("FromCardID")
                    .AsGuid()
                    .ForeignKey("FK_Card_To_CardTransfer_From", "Accounting", "Card", "CardID")
                    .NotNullable()
                .WithColumn("ToCardID")
                    .AsGuid()
                .ForeignKey("FK_Card_To_CardTransfer_To", "Accounting", "Card", "CardID")
                    .NotNullable()
                .WithColumn("Type").AsInt32().NotNullable().Indexed("IX_CardTransfer_Type");
        }

        public override void Down()
        {
            //Do nothing.
        }
    }
}
