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
                .WithColumn("Code").AsName().PrimaryKey("PK_OperationCategory")
                .WithColumn("Parent").AsName().Nullable()
                    .ForeignKey("FK_OperationCategory_To_OperationCategory", "Processing", "OperationCategory", "Code")
                    .Indexed("IX_OperationCategory_Parent")
                .WithColumn("Name").AsShortName().NotNullable()
                .WithColumn("Description").AsBigString().Nullable();

            Create.Table("Operation").InSchema(SchemaName)
                .WithColumn("ID").AsInt64().PrimaryKey("PK_Operation").Identity()
                .WithColumn("CategoryCode").AsName().NotNullable()
                    .ForeignKey("FK_OperationCategory_To_Operation", SchemaName, "OperationCategory", "Code")
                    .Indexed("IX_Operation_CategoryCode")
                .WithColumn("CreatedDateUtc").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("CompletedDateUtc").AsDateTime().Nullable()
                .WithColumn("ErrorMessage").AsBigString().Nullable()
                .WithColumn("Status").AsInt32().NotNullable()
                    .Indexed("IX_Operation_Status");

            Create.Table("Operation_User").InSchema(SchemaName)
                .WithColumn("OperationID").AsInt64().NotNullable().PrimaryKey("PK_Operation_User").ForeignKey("FK_Operation_To_Operation_User", SchemaName, "Operation", "ID")
                .WithColumn("UserID").AsGuid().NotNullable()
                    .ForeignKey("FK_User_To_Operation_User", "Membership", "User", "UserID");

            Create.Table("Transaction").InSchema(SchemaName)
                .WithColumn("TransactionID").AsGuid().PrimaryKey("PK_Transaction")
                .WithColumn("AccountNo").AsAccountNumber().NotNullable()
                    .ForeignKey("FK_Account_To_Transaction", "Accounting", "Account", "AccountNo")
                    .Indexed("IX_Transaction_AccountNo")
                .WithColumn("CurrencyISOName").AsCurrencyISOName().NotNullable()
                    .ForeignKey("FK_Currency_To_Transaction", "Accounting", "Currency", "CurrencyISOName")
                .WithColumn("TransactionAmount").AsDecimal().NotNullable()
                .WithColumn("AccountAmount").AsDecimal().NotNullable()
                .WithColumn("RemainingBalance").AsDecimal().NotNullable()
                .WithColumn("CreatedDateUtc").AsDateTime().NotNullable()
                .WithColumn("PostDateUtc").AsDateTime().Nullable()
                .WithColumn("Location").AsName().NotNullable()
                .WithColumn("Description").AsBigString().Nullable()
                .WithColumn("ErrorMessage").AsBigString().Nullable()
                .WithColumn("Status").AsInt32().NotNullable().Indexed("IX_Transaction_Status");

            Create.Table("CardTransaction").InSchema(SchemaName)
                .WithColumn("TransactionID").AsGuid().NotNullable().PrimaryKey("PK_CardTransaction")
                    .ForeignKey("FK_Transaction_To_CardTransaction", "Processing", "Transaction", "TransactionID")
                .WithColumn("CardID").AsGuid().NotNullable()
                    .ForeignKey("FK_Card_To_CardTransaction", "Accounting", "Card", "CardID")
                    .Indexed("IX_CardTransaction_CardID");

            Create.Table("Transfer").InSchema(SchemaName)
                .WithColumn("OperationID").AsInt64().PrimaryKey("PK_Transfer")
                    .ForeignKey("FK_Operation_To_Transfer", SchemaName, "Operation", "ID")
                .WithColumn("CurrencyISOName").AsCurrencyISOName().NotNullable()
                    .ForeignKey("FK_Currency_To_Transfer", "Accounting", "Currency", "CurrencyISOName")
                .WithColumn("FromAccountNo").AsAccountNumber().NotNullable()
                    .ForeignKey("FK_Account_To_Transfer_From", "Accounting", "Account", "AccountNo")
                .WithColumn("ToAccountNo").AsAccountNumber().NotNullable()
                    .ForeignKey("FK_Account_To_Transfer_To", "Accounting", "Account", "AccountNo")
                .WithColumn("Amount").AsDecimal().NotNullable()
                .WithColumn("FromTransactionID").AsGuid().Nullable()
                    .ForeignKey("FK_Transaction_To_Transfer_From", SchemaName, "Transaction", "TransactionID")
                .WithColumn("ToTransactionID").AsGuid().Nullable()
                    .ForeignKey("FK_Transaction_To_Transfer_To", SchemaName, "Transaction", "TransactionID");
                    
            Create.Table("CardTransfer").InSchema(SchemaName)
                .WithColumn("OperationID").AsInt64().PrimaryKey("PK_CardTransfer")
                    .ForeignKey("FK_Transfer_To_CardTransfer", SchemaName, "Transfer", "OperationID")
                .WithColumn("FromCardID").AsCardId().NotNullable()
                    .ForeignKey("FK_Card_To_CardTransfer_From", "Accounting", "Card", "CardID")
                .WithColumn("ToCardID").AsCardId().NotNullable()
                    .ForeignKey("FK_Card_To_CardTransfer_To", "Accounting", "Card", "CardID")
                .WithColumn("Type").AsInt32().NotNullable().Indexed("IX_CardTransfer_Type");
        }

        public override void Down()
        {
            Delete.Table("CardTransfer").InSchema(SchemaName);
            Delete.Table("Transfer").InSchema(SchemaName);
            Delete.Table("Transaction").InSchema(SchemaName);
            Delete.Table("Operation").InSchema(SchemaName);
            Delete.Table("OperationCategory").InSchema(SchemaName);
        }
    }
}
