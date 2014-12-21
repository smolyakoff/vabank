using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(56, "Update custom payment order template.")]
    public class UpdateCustomPaymentOrderTemplate : Migration
    {
        public override void Up()
        {
            Update.Table("PaymentTemplate").InSchema("Payments")
                .Set(new { FormTemplate = new ExplicitUnicodeString(Resource.ReadToEnd("M5_Release/Templates/custom-paymentorder.json"))})
                .Where(new { Code = "PAYMENT-CUSTOM-PAYMENTORDER" });
        }

        public override void Down()
        {
            //do nothing
        }
    }
}
