using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(48, "Seed payments data.")]
    [Tags("Development", "Test", "Production")]
    public class SeedPaymentsData : Migration
    {
        public override void Down()
        {            
        }

        public override void Up()
        {
            SeedCategoriesTree();
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

        private void SeedPaymentTemplates()
        {
            
        }

        private void SeedPaymentOrderTemplates()
        {
            Insert.IntoTable("PaymentOrderTemplate").InSchema("Payment")
                .Row(new {});
        }

    }
}
