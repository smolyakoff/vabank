using Dapper;
using FluentMigrator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace VaBank.Data.Migrations
{
    [Migration(48, "Seed payments data.")]
    [Tags("Development", "Test", "Production")]
    public class SeedPaymentsData : Migration
    {
        private const string VaBankPrefix = "3014"; 
        
        public override void Down()
        {            
        }

        public override void Up()
        {
            SeedCategoriesTree();
            SeedCorrespondentAccounts();
            SeedPaymentTemplates();
            SeedPaymentOrderTemplates();
        }

        private void SeedCategoriesTree()
        {
            Insert.IntoTable("OperationCategory").InSchema("Processing")
                .Row(new { Code = "PAYMENT", Name = new ExplicitUnicodeString("Платёж") })
                .Row(new { Code = "PAYMENT-CUSTOM", Name = new ExplicitUnicodeString("Произвольный платёж"), Parent = "PAYMENT" })
                .Row(new { Code = "PAYMENT-CUSTOM-PAYMENTORDER", Name = new ExplicitUnicodeString("Платёж по реквизитам"), Parent = "PAYMENT-CUSTOM" })
                .Row(new { Code = "PAYMENT-CELL", Name = new ExplicitUnicodeString("Мобильная связь"), Parent = "PAYMENT" })
                .Row(new { Code = "PAYMENT-CELL-VELCOM", Name = "Velcom", Parent = "PAYMENT-CELL" })
                .Row(new { Code = "PAYMENT-CELL-VELCOM-PHONENO", Name = new ExplicitUnicodeString("По номеру телефона"), Parent = "PAYMENT-CELL-VELCOM" });
        }

        private void SeedCorrespondentAccounts()
        {
            Execute.WithConnection((connection, transaction) => 
            {
                var codes = connection.Query<string>("SELECT [Code] FROM [Accounting].[Bank] WHERE [Code] <> '153001966'", null, transaction).ToList();
                foreach (var code in codes)
                {
                    var accountNo = string.Format("{0}{1}", VaBankPrefix, code);
                    connection.Execute("INSERT INTO [Accounting].[Account] ([AccountNo],[CurrencyISOName],[OpenDateUtc],[ExpirationDateUtc],[Type]) VALUES (@AccountNo,@CurrencyISOName,@OpenDateUtc,@ExpirationDateUtc,@Type)",
                        new { AccountNo = accountNo, CurrencyISOName = "BYR", OpenDateUtc = DateTime.UtcNow, ExpirationDateUtc = DateTime.UtcNow.AddYears(1), Type = "CorrespondentAccount" }, transaction);
                    connection.Execute("INSERT INTO [Accounting].[CorrespondentAccount] ([AccountNo],[BankCode]) VALUES (@AccountNo,@BankCode)", new { AccountNo = accountNo, BankCode = code }, transaction);
                }
            });
        }

        private void SeedPaymentTemplates()
        {
            var assembly = Assembly.GetExecutingAssembly();            
            Insert.IntoTable("PaymentTemplate").InSchema("Payments")
                .Row(new { Code = "PAYMENT-CELL-VELCOM-PHONENO", FormTemplate = new ExplicitUnicodeString(JObject.Parse(new StreamReader(assembly.GetManifestResourceStream("VaBank.Data.Migrations.M4_Payments.Templates.cell-velcom-phoneno.json")).ReadToEnd()).ToString(Formatting.None)) })
                .Row(new { Code = "PAYMENT-CUSTOM-PAYMENTORDER", FormTemplate = new ExplicitUnicodeString(JObject.Parse(new StreamReader(assembly.GetManifestResourceStream("VaBank.Data.Migrations.M4_Payments.Templates.custom-paymentorder.json")).ReadToEnd()).ToString(Formatting.None)) });
        }

        private void SeedPaymentOrderTemplates()
        {
            Insert.IntoTable("PaymentOrderTemplate").InSchema("Payments")
                .Row(new 
                {
                    PaymentTemplateCode = "PAYMENT-CELL-VELCOM-PHONENO",
                    PayerName = "{{payer.name}}",
                    PayerTIN = "{{payer.tin}}",
                    PayerBankCode = "{{payer.bankCode}}",
                    PayerAccountNo = "{{payer.accountNo}}",
                    BeneficiaryName =  new ExplicitUnicodeString("Унитарное предприятие по оказанию услуг \"Велком\""),
                    BeneficiaryBankCode = "153001749",
                    BeneficiaryAccountNo = "3012202410089",
                    BeneficiaryTIN = "101528843",
                    Amount = "{{form.amount}}",
                    Purpose = new ExplicitUnicodeString("Пополнение счета. Номер телефона: {{form.phoneNo}}"),
                    CurrencyISOName = "BYR"
                })
                .Row(new 
                {
                    PaymentTemplateCode = "PAYMENT-CUSTOM-PAYMENTORDER",
                    PayerName = "{{payer.name}}",
                    PayerTIN = "{{payer.tin}}",
                    PayerBankCode = "{{payer.bankCode}}",
                    PayerAccountNo = "{{payer.accountNo}}",
                    BeneficiaryName =  "{{form.beneficiaryName}}",
                    BeneficiaryBankCode = "{{form.beneficiaryBankCode}}",
                    BeneficiaryAccountNo = "{{form.beneficiaryAccountNo}}",
                    BeneficiaryTIN = "{{form.beneficiaryTIN}}",
                    Amount = "{{form.amount}}",
                    Purpose = "{{form.purpose}}",
                    CurrencyISOName = "BYR"
                });
        }

    }
}
