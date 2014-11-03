using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(31, "Add precision column to [Accounting].[Currency] table.")]
    [Tags("Development", "Test", "Production")]
    public class AddPrecisionToCurrency : Migration
    {
        public override void Down()
        {            
        }

        public override void Up()
        {
            Alter.Table("Currency").InSchema("Accounting").AddColumn("Precision").AsInt32().NotNullable().WithDefaultValue(2);
            Update.Table("Currency").InSchema("Accounting").Set(new { Precision = 0 }).Where(new { CurrencyISOName = "BYR" });
        }
    }
}
