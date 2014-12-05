using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .WithColumn("Code").AsBankCode().PrimaryKey("PK_Bank")
                .WithColumn("Name").AsName().NotNullable()
                .WithColumn("Parent").AsBankCode().ForeignKey("FK_Bank_To_ParentBank", "Accounting", "Bank", "Code").Nullable();

            Create.Table("CorrespondentAccount").InSchema("Accounting")
                .WithColumn("AccountNo")
                    .AsAccountNumber()
                    .PrimaryKey("PK_CorrespondentAccount")
                    .ForeignKey("FK_CorrespondentAccount_To_Account", "Accounting", "Account", "AccountNo")
                .WithColumn("BankCode")
                    .AsBankCode()
                    .ForeignKey("FK_CorrespondentAccount_To_Bank", "Accounting", "Bank", "Code")
                    .NotNullable();

            Create.Table("PaymentOrder").InSchema(SchemaName)
                .WithColumn("PaymentOrderNo").AsInt64().PrimaryKey("PK_PaymentOrder").Identity()
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
                .WithColumn("CurrencyISOName").AsCurrencyISOName()
                .WithColumn("PaymentCode").AsPaymentCode().NotNullable();

            Create.Table("PaymentTransaction").InSchema(SchemaName)
                .WithColumn("TransactionId")
                    .AsGuid()
                    .PrimaryKey("PK_PaymentTransaction")
                    .ForeignKey("FK_PaymentTransaction_To_Transaction", "Processing", "Transaction", "TransactionID")
                .WithColumn("PaymentOrderNo").AsInt64().ForeignKey("FK_PaymentTransaction_To_PaymentOrder", SchemaName, "PaymentOrder", "PaymentOrderNo");

            Create.Table("PaymentTemplate").InSchema(SchemaName)
                .WithColumn("Code").AsPaymentTemplateCode().PrimaryKey("PK_PaymentTemplate")
                .WithColumn("CategoryCode").AsName().ForeignKey("FK_PaymentTemplate_To_OperationCategory", "Processing", "OperationCategory", "Code").NotNullable()
                .WithColumn("AccountNo").AsAccountNumber().ForeignKey("PaymentTemplate_To_Account", "Accounting", "Account", "AccountNo").NotNullable()
                .WithColumn("CurrencyISOName").AsCurrencyISOName().ForeignKey("FK_PaymentTemplate_To_Currency", "Accounting", "Currency", "CurrencyISOName").NotNullable()
                .WithColumn("Name").AsName().NotNullable()
                .WithColumn("ValidatorName").AsName().NotNullable();

            Create.Table("Payment").InSchema(SchemaName)
                .WithColumn("OperationId")
                    .AsInt64()
                    .PrimaryKey("PK_Payment")
                    .ForeignKey("FK_Payment_To_Transfer", "Processing", "Transfer", "OperationId")
                    .NotNullable()
               .WithColumn("OrderNo").AsInt64().ForeignKey("FK_Payment_To_PaymentOrder", SchemaName, "PaymentOrder", "PaymentOrderNo")
               .WithColumn("TemplateCode")
                   .AsPaymentTemplateCode().ForeignKey("FK_Payment_To_PaymentTemplate", SchemaName, "PaymentTemplate", "Code")
               .WithColumn("Name").AsBigString().NotNullable()
               .WithColumn("Form").AsString(int.MaxValue).NotNullable();

            Create.Table("CardPayment").InSchema(SchemaName)
                .WithColumn("OperationId")
                    .AsInt64()
                    .PrimaryKey("PK_CardPayment")
                    .ForeignKey("FK_CardPayment_To_Payment", SchemaName, "Payment", "OperationId")
                .WithColumn("CardId")
                    .AsCardId()
                    .ForeignKey("FK_CardPayment_To_Card", "Accounting", "Card", "CardID")
                    .NotNullable();

            Create.Table("PaymentOrderTemplate").InSchema(SchemaName)
                .WithColumn("PaymentTemplateCode")
                    .AsPaymentTemplateCode()
                    .PrimaryKey("PK_PaymentOrderTemplate")
                    .ForeignKey("FK_PK_PaymentOrderTemplate_To_PaymentTemplate", SchemaName, "PaymentTemplate", "Code")
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
                .WithColumn("CurrencyISOName").AsCurrencyISOName().NotNullable();

            Create.Table("UserPaymentProfile").InSchema(SchemaName)
                .WithColumn("UserId")
                    .AsUserId()
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
