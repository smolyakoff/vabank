using FluentMigrator;

namespace VaBank.Data.Migrations.M4_Payments
{
    [Migration(47, "Seed VaBank bank.")]
    [Tags("Development", "Test", "Production")]
    public class SeedBanks: Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Bank").InSchema("Accounting").Row(new { Code = "153005042", Name = "Национальный Банк Республики Беларусь" });
            Insert.IntoTable("Bank").InSchema("Accounting").Row(new { Code = "153001966", Name = "VaBank", Parent = "153005042" });
            Insert.IntoTable("Bank").InSchema("Accounting").Row(new { Code = "153001795", Name = "ОАО \"АСБ Беларусбанк\"", Parent = "153005042" });
            Insert.IntoTable("Bank").InSchema("Accounting").Row(new { Code = "153001110", Name = "ЗАО \"РРБ Банк\"", Parent = "153005042" });
            Insert.IntoTable("Bank").InSchema("Accounting").Row(new { Code = "153001175", Name = "ЗАО \"БелСвиссБанк\"", Parent = "153005042" });
        }

        public override void Down()
        {
        }
    }
}
