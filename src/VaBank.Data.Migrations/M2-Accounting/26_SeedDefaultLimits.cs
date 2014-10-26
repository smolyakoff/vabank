using FluentMigrator;
using Newtonsoft.Json;

namespace VaBank.Data.Migrations
{
    [Migration(26, "Seed default card limits.")]
    [Tags("Development", "Test", "Production")]
    public class SeedDefaultLimits : Migration
    {
        private const string Key = "VaBank.Accounting.CardLimits.Default.{0}";

        public override void Up()
        {
            Insert.IntoTable("Setting").InSchema("App")
                .Row(UsdLimits())
                .Row(EuroLimits())
                .Row(ByrLimits());
        }

        public override void Down()
        {
            //do nothing
        }

        private static object ByrLimits()
        {
            var limits = new
            {
                LimitOperationsPerDayLocal = 20,
                LimitOperationsPerDayAbroad = 10,
                LimitAmountPerDayLocal = 6000000.0m,
                LimitAmountPerDayAbroad = 3000000.0m
            };
            var json = JsonConvert.SerializeObject(limits);
            var node = JsonConvert.DeserializeXNode(json, "Setting");
            return new {Key = string.Format(Key, "BYR"), Value = node.ToString()};
        }

        private static object EuroLimits()
        {
            var limits = new
            {
                LimitOperationsPerDayLocal = 20,
                LimitOperationsPerDayAbroad = 10,
                LimitAmountPerDayLocal = 1000m,
                LimitAmountPerDayAbroad = 500m
            };
            var json = JsonConvert.SerializeObject(limits);
            var node = JsonConvert.DeserializeXNode(json, "Setting");
            return new {Key = string.Format(Key, "EUR"), Value = node.ToString()};
        }

        private static object UsdLimits()
        {
            var limits = new
            {
                LimitOperationsPerDayLocal = 20,
                LimitOperationsPerDayAbroad = 10,
                LimitAmountPerDayLocal = 1000m,
                LimitAmountPerDayAbroad = 500m
            };
            var json = JsonConvert.SerializeObject(limits);
            var node = JsonConvert.DeserializeXNode(json, "Setting");
            return new {Key = string.Format(Key, "USD"), Value = node.ToString()};
        }
    }
}
