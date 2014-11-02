using FluentMigrator;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace VaBank.Data.Migrations
{
    [Migration(24, "Seed basic currencies.")]
    [Tags("Accounting", "Development", "Production", "Test")]
    public class SeedBasicCurrencies : Migration
    {
        public override void Down()
        {
            //Do nothing
        }

        public override void Up()
        {     
            var currencies = new List<M2Currency>
                {
                    M2Currency.Create("USD", "Доллар Соединённых Штатов Америки", "$"),
                    M2Currency.Create("EUR", "Евро", "\u20AC"),
                    M2Currency.Create("BYR", "Белорусский рубль", "Br")
                };

            currencies.ForEach(currency =>
                Insert.IntoTable("Currency").InSchema("Accounting").Row(currency));
        }
    }
}
