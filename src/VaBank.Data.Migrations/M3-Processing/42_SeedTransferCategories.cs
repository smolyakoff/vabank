using FluentMigrator;

namespace VaBank.Data.Migrations.M3_Processing
{
    [Migration(42, "Seed default transfer categories.")]
    [Tags("Development", "Production", "Test")]
    public class SeedTransferCategories : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("OperationCategory").InSchema("Processing")
                .Row(new {Code = "TRANSFER", Name = new ExplicitUnicodeString("Перевод"), Description = new ExplicitUnicodeString("Перевод с одного банковского счета на другой")})
                .Row(new {Code = "TRANSFER-CARD", Name = new ExplicitUnicodeString("Перевод с карты на карту"), Parent = "TRANSFER"})
                .Row(new {Code = "TRANSFER-CARD-PERSONAL", Name = new ExplicitUnicodeString("Перевод на свою карту"), Parent = "TRANSFER-CARD"})
                .Row(new {Code = "TRANSFER-CARD-INTERBANK", Name = new ExplicitUnicodeString("Перевод на карту другого клиента"), Parent = "TRANSFER-CARD"});
        }

        public override void Down()
        {
            //Do nothing
        }
    }
}