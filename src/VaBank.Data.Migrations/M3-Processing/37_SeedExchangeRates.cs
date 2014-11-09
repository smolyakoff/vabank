using System;
using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(37, "Seed basic exchange rates.")]
    [Tags("Development", "Test", "Production")]
    public class SeedExchangeRates : Migration
    {
        public override void Up()
        {
            var timestamp = new DateTime(2014, 11, 7, 19, 0, 0, 0, DateTimeKind.Utc);
            Insert.IntoTable("ExchangeRate").InSchema("Processing")
                .Row(new
                {
                    ExchangeRateID = Guid.NewGuid(),
                    BaseCurrencyISOName = "BYR",
                    ForeignCurrencyISOName = "USD",
                    BuyRate = 10710m,
                    SellRate = 10770m,
                    TimestampUtc = timestamp,
                    IsActual = true
                })
                .Row(new
                {
                    ExchangeRateID = Guid.NewGuid(),
                    BaseCurrencyISOName = "BYR",
                    ForeignCurrencyISOName = "EUR",
                    BuyRate = 13320m,
                    SellRate = 13520m,
                    TimestampUtc = timestamp,
                    IsActual = true
                })
                .Row(new
                {
                    ExchangeRateID = Guid.NewGuid(),
                    BaseCurrencyISOName = "USD",
                    ForeignCurrencyISOName = "EUR",
                    BuyRate = 0.8m,
                    SellRate = 1.25m,
                    TimestampUtc = timestamp,
                    IsActual = true
                });

        }

        public override void Down()
        {
            //do nothing
        }
    }
}
