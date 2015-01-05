using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(58, "Fixed bug with display name of byfly payment.")]
    [Tags("Development", "Test", "Production")]
    public class FixedUnicodeForTemplateName : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"UPDATE [Payments].[PaymentTemplate]
                          SET [DisplayName] = N'Интернет - BYFLY'
                          WHERE [Code] = 'PAYMENT-INTERNET-BYFLY'");
        }

        public override void Down()
        {
            //do nothing
        }
    }
}
