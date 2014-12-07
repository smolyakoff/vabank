using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(46, "Add payments schema tables.")]
    public class Payments: Migration
    {
        private const string SchemaName = "Payments";

        public override void Up()
        {
            Create.Schema(SchemaName);

            Create.Table("Bank").InSchema("Accounting")
                .WithColumn("Code").AsBankCode()
                    .PrimaryKey("PK_Bank")
                .WithColumn("Name").AsName().NotNullable()
                .WithColumn("Parent").AsBankCode().Nullable()
                    .ForeignKey("FK_Bank_To_ParentBank", "Accounting", "Bank", "Code");

            Create.Table("CorrespondentAccount").InSchema("Accounting")
                .WithColumn("AccountNo").AsAccountNumber()
                    .PrimaryKey("PK_CorrespondentAccount")
                    .ForeignKey("FK_CorrespondentAccount_To_Account", "Accounting", "Account", "AccountNo")
                .WithColumn("BankCode").AsBankCode().NotNullable()
                    .ForeignKey("FK_CorrespondentAccount_To_Bank", "Accounting", "Bank", "Code");
                    
            Create.Table("PaymentOrder").InSchema(SchemaName)
                .WithColumn("No").AsInt64()
                    .PrimaryKey("PK_PaymentOrder").Identity()
                .WithColumn("PayerName").AsName().NotNullable()
                .WithColumn("PayerBankCode").AsBankCode().NotNullable()
                .WithColumn("PayerAccountNo").AsAccountNumber().NotNullable()
                .WithColumn("PayerTIN").AsTIN().NotNullable()
                .WithColumn("BeneficiaryName").AsName().NotNullable()
                .WithColumn("BeneficiaryBankCode").AsBankCode().NotNullable()
                .WithColumn("BeneficiaryAccountNo").AsAccountNumber().NotNullable()
                .WithColumn("BeneficiaryTIN").AsTIN().NotNullable()
                .WithColumn("Purpose").AsBigString().NotNullable()
                .WithColumn("Amount").AsDecimal().NotNullable()
                .WithColumn("CurrencyISOName").AsCurrencyISOName().NotNullable()
                .WithColumn("PaymentCode").AsPaymentCode().NotNullable();

            Create.Table("PaymentTransaction").InSchema(SchemaName)
                .WithColumn("TransactionId").AsGuid()
                    .PrimaryKey("PK_PaymentTransaction")
                    .ForeignKey("FK_PaymentTransaction_To_Transaction", "Processing", "Transaction", "TransactionID")
                .WithColumn("PaymentOrderNo").AsInt64().ForeignKey("FK_PaymentTransaction_To_PaymentOrder", SchemaName, "PaymentOrder", "No");

            Create.Table("PaymentTemplate").InSchema(SchemaName)
                .WithColumn("Code").AsName()
                    .PrimaryKey("PK_PaymentTemplate")
                    .ForeignKey("FK_PaymentTemplate_To_OperationCategory", "Processing", "OperationCategory", "Code")
                .WithColumn("AccountNo").AsBigString().NotNullable()
                .WithColumn("CurrencyISOName").AsBigString().NotNullable()
                .WithColumn("FormTemplate").AsText().NotNullable();

            Create.Table("Payment").InSchema(SchemaName)
                .WithColumn("OperationId").AsInt64().NotNullable()
                    .PrimaryKey("PK_Payment")
                    .ForeignKey("FK_Payment_To_Transfer", "Processing", "Transfer", "OperationId")
               .WithColumn("OrderNo").AsInt64().NotNullable()
                    .ForeignKey("FK_Payment_To_PaymentOrder", SchemaName, "PaymentOrder", "No")
               .WithColumn("Form").AsText().NotNullable();

            Create.Table("CardPayment").InSchema(SchemaName)
                .WithColumn("OperationId").AsInt64()
                    .PrimaryKey("PK_CardPayment")
                    .ForeignKey("FK_CardPayment_To_Payment", SchemaName, "Payment", "OperationId")
                .WithColumn("CardId").AsCardId().NotNullable()
                    .ForeignKey("FK_CardPayment_To_Card", "Accounting", "Card", "CardID");
                    
            Create.Table("PaymentOrderTemplate").InSchema(SchemaName)
                .WithColumn("PaymentTemplateCode")
                    .AsName()
                    .PrimaryKey("PK_PaymentOrderTemplate")
                    .ForeignKey("FK_PaymentOrderTemplate_To_PaymentTemplate", SchemaName, "PaymentTemplate", "Code")
                .WithColumn("PayerName").AsBigString().NotNullable()
                .WithColumn("PayerBankCode").AsBigString().NotNullable()
                .WithColumn("PayerAccountNo").AsBigString().NotNullable()
                .WithColumn("PayerTIN").AsBigString().NotNullable()
                .WithColumn("BeneficiaryName").AsBigString().NotNullable()
                .WithColumn("BeneficiaryBankCode").AsBigString().NotNullable()
                .WithColumn("BeneficiaryAccountNo").AsBigString().NotNullable()
                .WithColumn("BeneficiaryTIN").AsBigString().NotNullable()
                .WithColumn("Purpose").AsBigString().NotNullable()
                .WithColumn("Amount").AsBigString().NotNullable()
                .WithColumn("CurrencyISOName").AsBigString().NotNullable();

            Create.Table("UserPaymentProfile").InSchema(SchemaName)
                .WithColumn("UserId").AsUserId()
                    .PrimaryKey("PK_UserPaymentProfile")
                    .ForeignKey("FK_UserPaymentProfile_To_User", "Membership", "User", "UserID")
                .WithColumn("FullName").AsName().NotNullable()
                .WithColumn("Address").AsBigString().NotNullable()
                .WithColumn("PayerTIN").AsTIN().NotNullable();
        }

        public override void Down()
        {
        }
    }
}
