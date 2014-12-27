using System;
using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(57, "Add Internet payments.")]
    [Tags("Development", "Test", "Production")]
    public class InternetPayments : Migration
    {
        public override void Up()
        {
            SeedCategories();
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
        }
    }
}
