using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Data.Migrations
{
    [Migration(34, "Add ExchangeRate table to [Accounting] schema.")]
    [Tags("Development", "Test", "Production")]
    public class AddExchangeRateTable : Migration
    {
        private const string SchemaName = "Accounting";
        public override void Down()
        {
            Delete.Table("ExchangeRate").InSchema(SchemaName);
        }

        public override void Up()
        {
            Create.Table("ExchangeRate").InSchema(SchemaName)
                .WithColumn("ExchangeRateID").AsGuid().PrimaryKey("PK_Currency")
                .WithColumn("FromCurrencyISOName").AsCurrencyISOName().ForeignKey("FK_ExchangeRate_To_FromCurrency", SchemaName, "Currency", "CurrencyISOName")
                .WithColumn("ToCurrencyISOName").AsCurrencyISOName().ForeignKey("FK_ExchangeRate_To_ToCurrency", SchemaName, "Currency", "CurrencyISOName")
                .WithColumn("BuyRate").AsDecimal().NotNullable()
                .WithColumn("SellRate").AsDecimal().NotNullable()
                .WithColumn("TimeStampUtc").AsDateTime().NotNullable()
                .WithColumn("IsActual").AsBoolean().NotNullable().Indexed("IX_IsActual");
        }
    }
}
