using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(39, "Add russian currency.")]
    [Tags("Accounting", "Development", "Production", "Test")]
    public class AddRussianCurrency : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Currency").InSchema("Accounting")
                .Row(new
                {
                    CurrencyISOName = "RUB",
                    Name = new ExplicitUnicodeString("Российский рубль"),
                    Symbol = new ExplicitUnicodeString("₽"),
                    Precision = 2
                });
        }

        public override void Down()
        {
            //Do nothing
        }
    }
}
