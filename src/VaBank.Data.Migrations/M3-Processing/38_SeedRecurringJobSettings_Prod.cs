using FluentMigrator;
using Newtonsoft.Json;

namespace VaBank.Data.Migrations
{
    [Migration(38, "Seed settings for recurring jobs.")]
    [Tags("Production")]
    public class SeedRecurringJobSettings_Prod : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Setting").InSchema("App")
                .Row(new { Key = "VaBank.Jobs.Recurring.KeepAlive", Value = Serialize(true, "*/10 * * * *") })
                .Row(new { Key = "VaBank.Jobs.Recurring.UpdateExchangeRates", Value = Serialize(true, "0 19 * * 1,2,3,4,5") });

        }

        public override void Down()
        {
            //do nothing
        }

        private static string Serialize(bool enabled, string cron)
        {
            return JsonConvert.SerializeObject(new {Enabled = enabled, Cron = cron});
        }
    }
}
