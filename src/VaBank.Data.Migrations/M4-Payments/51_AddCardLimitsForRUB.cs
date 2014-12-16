using FluentMigrator;
using Newtonsoft.Json;
using VaBank.Common.Util;

namespace VaBank.Data.Migrations
{
    [Migration(51, "Add card limits for RUB")]
    [Tags("Development", "Test", "Production")]
    public class AddCardLimitsForRub : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Setting").InSchema("App")
                .Row(new
                {
                    Key = "VaBank.Accounting.CardLimits.Default.RUB",
                    Value = JsonConvert.SerializeObject(new
                    {
                        OperationPerDayLocal = 20,
                        OperationsPerDayAbroad = 10,
                        AmountPerDayLocal = 3000,
                        AmountPerDayAbroad = 1500
                    })
                });
            Insert.IntoTable("Setting").InSchema("App")
                .Row(new
                {
                    Key = "VaBank.Accounting.CardLimits.Range.RUB",
                    Value = JsonConvert.SerializeObject(new
                    {
                        AmountPerDayLocal = Range.Create(50m, 500000m),
                        AmountPerDayAbroad = Range.Create(50m, 250000m),
                        OperationsPerDayLocal = Range.Create(0, 500),
                        OperationsPerDayAbroad = Range.Create(0, 250),
                    })
                });
        }

        public override void Down()
        {
            //do nothing
        }
    }
}
