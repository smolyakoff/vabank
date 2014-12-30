using FluentMigrator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VaBank.Data.Migrations
{
    [Migration(57, "Add Internet payments.")]
    [Tags("Development", "Test", "Production")]
    public class InternetPayments : Migration
    {
        public override void Up()
        {
            SeedCategories();
            SeedPaymentTemplates();
            SeedPaymentOrderTemplates();
        }

        public override void Down()
        {
        }

        private void SeedCategories()
        {
            Insert.IntoTable("OperationCategory").InSchema("Accounting")
                .Row(
                    new
                    {
                        Code = "PAYMENT-INTERNET",
                        Name = new ExplicitUnicodeString("Интернет"),
                        Parent = "PAYMENT"
                    });
            Insert.IntoTable("OperationCategory").InSchema("Accounting")
                .Row(
                    new
                    {
                        Code = "PAYMENT-INTERNET-BYFLY",
                        Name = "BYFLY",
                        Parent = "PAYMENT-INTERNET"
                    });
        }

        private void SeedPaymentTemplates()
        {
            Insert.IntoTable("PaymentTemplate").InSchema("Payments")
                .Row(new
                {
                    Code = "PAYMENT-INTERNET-BYFLY",
                    FormTemplate =
                        new ExplicitUnicodeString(
                            JObject.Parse(Resource.ReadToEnd("M5_Release/Templates/internet-byfly.json"))
                                .ToString(Formatting.None)),
                    InfoTemplate =
                        new ExplicitUnicodeString(Resource.ReadToEnd("M5_Release/Templates/internet-byfly.info.txt")),
                    DisplayName = "BYFLY"
                });
        }

        private void SeedPaymentOrderTemplates()
        {
            Insert.IntoTable("PaymentOrderTemplate").InSchema("Payments")
                .Row(new
                {
                    PaymentTemplateCode = "PAYMENT-INTERNET-BYFLY",
                    PayerName = "{{form.payerName}}",
                    PayerTIN = "{{form.payerTin}}",
                    PayerBankCode = "{{form.payerBankCode}}",
                    PayerAccountNo = "{{form.payerAccountNo}}",
                    BeneficiaryName = new ExplicitUnicodeString("Республиканское унитарное предприятие \"Белтелеком\""),
                    BeneficiaryBankCode = "153001749",
                    BeneficiaryAccountNo = "3012016182011",
                    BeneficiaryTIN = "191124936",
                    Amount = "{{form.amount}}",
                    Purpose = new ExplicitUnicodeString("Пополнение счета BYFLY. Номер счёта: {{form.accountNo}}"),
                    CurrencyISOName = "BYR"
                });
        }
    }
}
