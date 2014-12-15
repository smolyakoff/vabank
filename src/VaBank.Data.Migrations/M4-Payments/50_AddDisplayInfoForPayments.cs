using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(50, "Add info template for payment templates.")]
    [Tags("Development", "Test", "Production")]
    public class AddDisplayInfoForPayments : Migration
    {
        public override void Up()
        {
            Alter.Table("PaymentTemplate").InSchema("Payments")
                .AddColumn("InfoTemplate").AsString(4096).Nullable();

            Update.Table("PaymentTemplate").InSchema("Payments")
                .Set(new {InfoTemplate = new ExplicitUnicodeString(Resource.ReadToEnd("M4_Payments/Templates/cell-velcom-phoneno.info.txt"))})
                .Where(new {Code = "PAYMENT-CELL-VELCOM-PHONENO"});

            Update.Table("PaymentTemplate").InSchema("Payments")
                .Set(new { InfoTemplate = new ExplicitUnicodeString(Resource.ReadToEnd("M4_Payments/Templates/custom-paymentorder.info.txt")) })
                .Where(new { Code = "PAYMENT-CUSTOM-PAYMENTORDER" });

            Alter.Table("Payment").InSchema("Payments")
                .AddColumn("Info").AsString(1024).Nullable();
        }

        public override void Down()
        {
            Delete.Column("InfoTemplate").FromTable("PaymentTemplate").InSchema("Payments");
            Delete.Column("Info").FromTable("Payment").InSchema("Payments");
        }
    }
}
