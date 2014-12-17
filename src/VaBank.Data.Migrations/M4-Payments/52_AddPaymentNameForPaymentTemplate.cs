using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(52, "Added payment display name to payment template.")]
    public class AddPaymentNameForPaymentTemplate : Migration
    {
        public override void Up()
        {
            Alter.Table("PaymentTemplate").InSchema("Payments")
                .AddColumn("DisplayName").AsString(100).NotNullable()
                .SetExistingRowsTo("<PLACEHOLDER>");

            Update.Table("PaymentTemplate").InSchema("Payments")
                .Set(new { DisplayName = new ExplicitUnicodeString("Velcom - По номеру телефона") })
                .Where(new { Code = "PAYMENT-CELL-VELCOM-PHONENO" });

            Update.Table("PaymentTemplate").InSchema("Payments")
                .Set(new { DisplayName = new ExplicitUnicodeString("Платеж по реквизитам") })
                .Where(new { Code = "PAYMENT-CUSTOM-PAYMENTORDER" });
        }

        public override void Down()
        {
            Delete.Column("DisplayName").FromTable("PaymentTemplate").InSchema("Payments");
        }
    }
}
