using FluentMigrator;

namespace VaBank.Data.Migrations
{
    [Migration(34, "Add ExchangeRate table to [Processing] schema.")]
    [Tags("Development", "Test", "Production")]
    public class AddExchangeRateTable : Migration
    {
        private const string SchemaName = "Processing";
        public override void Down()
        {
            Delete.Table("ExchangeRate").InSchema(SchemaName);
        }

        public override void Up()
        {
            Create.Schema(SchemaName);
            Create.Table("ExchangeRate").InSchema(SchemaName)
                .WithColumn("ExchangeRateID").AsGuid().PrimaryKey("PK_ExchangeRate")
                .WithColumn("BaseCurrencyISOName").AsCurrencyISOName().ForeignKey("FK_ExchangeRate_To_FromCurrency", "Accounting", "Currency", "CurrencyISOName")
                .WithColumn("ForeignCurrencyISOName").AsCurrencyISOName().ForeignKey("FK_ExchangeRate_To_ToCurrency", "Accounting", "Currency", "CurrencyISOName")
                .WithColumn("BuyRate").AsDecimal().NotNullable()
                .WithColumn("SellRate").AsDecimal().NotNullable()
                .WithColumn("TimestampUtc").AsDateTime().NotNullable()
                .WithColumn("IsActual").AsBoolean().NotNullable().Indexed("IX_ExchangeRate_IsActual");

            Create.Index("IX_ExchangeRate_AK").OnTable("ExchangeRate").InSchema("Processing")
                .OnColumn("TimestampUtc").Descending()
                .OnColumn("BaseCurrencyISOName").Ascending()
                .OnColumn("ForeignCurrencyISOName").Ascending()
                .WithOptions().Unique();
        }
    }
}
