using FluentMigrator;
using Newtonsoft.Json;
using VaBank.Common.Util;

namespace VaBank.Data.Migrations
{
    [Migration(55, "Update card limits for RUB")]
    [Tags("Development", "Test", "Production")]
    public class UpdateCardLimitsForRUB : Migration
    {
        public override void Up()
        {
            Update.Table("Setting").InSchema("App")
                .Set(new
                {
                    Value = JsonConvert.SerializeObject(new
                    {
                        OperationsPerDayLocal = 20,
                        OperationsPerDayAbroad = 10,
                        AmountPerDayLocal = 60000,
                        AmountPerDayAbroad = 30000
                    })
                })
                .Where(new
                {
                    Key = "VaBank.Accounting.CardLimits.Default.RUB"
                });
            Update.Table("Setting").InSchema("App")
                .Set(new
                {
                    Value = JsonConvert.SerializeObject(new
                    {
                        AmountPerDayLocal = Range.Create(50m, 6000000m),
                        AmountPerDayAbroad = Range.Create(50m, 300000m),
                        OperationsPerDayLocal = Range.Create(0, 500),
                        OperationsPerDayAbroad = Range.Create(0, 250),
                    })
                })
                .Where(new
                {
                    Key = "VaBank.Accounting.CardLimits.Range.RUB"
                });
        }

        public override void Down()
        {
            //do nothing
        }
    }
}
