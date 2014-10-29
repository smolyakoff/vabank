using FluentMigrator;
using Newtonsoft.Json;
using VaBank.Common.Util;

namespace VaBank.Data.Migrations
{
    [Migration(28, "Seed range card limits.")]
    [Tags("Development", "Test", "Production")]
    public class SeedRangeLimits : Migration
    {
        private const string Key = "VaBank.Accounting.CardLimits.Range.{0}";
        
        public override void Down()
        {
            //Nothing to do
        }

        public override void Up()
        {
            Insert.IntoTable("Setting").InSchema("App")
                .Row(UsdRange())
                .Row(EuroRange())
                .Row(ByrRange());
        }

        private static object ByrRange()
        {
            var limits = new
            {
                AmountPerDayLocal = Range.Create<decimal>(10000m, 100000000m),
                AmountPerDayAbroad = Range.Create<decimal>(10000m, 50000000m),
                OperationsPerDayLocal = Range.Create(0, 500),
                OperationsPerDayAbroad = Range.Create(0, 250),
            };
            var json = JsonConvert.SerializeObject(limits);
            var node = JsonConvert.DeserializeXNode(json, "Setting");
            return new { Key = string.Format(Key, "BYR"), Value = node.ToString() };
        }

        private static object EuroRange()
        {
            var limits = new
            {
                AmountPerDayLocal = Range.Create<decimal>(5m, 100000m),
                AmountPerDayAbroad = Range.Create<decimal>(5m, 50000m),
                OperationsPerDayLocal = Range.Create(0, 500),
                OperationsPerDayAbroad = Range.Create(0, 250),
            };
            var json = JsonConvert.SerializeObject(limits);
            var node = JsonConvert.DeserializeXNode(json, "Setting");
            return new { Key = string.Format(Key, "EUR"), Value = node.ToString() };
        }

        private static object UsdRange()
        {
            var limits = new
            {
                AmountPerDayLocal = Range.Create<decimal>(5m, 100000m),
                AmountPerDayAbroad = Range.Create<decimal>(5m, 50000m),
                OperationsPerDayLocal = Range.Create(0, 500),
                OperationsPerDayAbroad = Range.Create(0, 250),
            };
            var json = JsonConvert.SerializeObject(limits);
            var node = JsonConvert.DeserializeXNode(json, "Setting");
            return new { Key = string.Format(Key, "USD"), Value = node.ToString() };
        }
    }
}
