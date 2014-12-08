using FluentMigrator;

namespace VaBank.Data.Migrations.M4_Payments
{
    [Migration(47, "Seed banks.")]
    [Tags("Development", "Test", "Production")]
    public class SeedBanks: Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Bank").InSchema("Accounting")
                .Row(new { Code = "153005042", Name = new ExplicitUnicodeString("Национальный Банк Республики Беларусь") })
                .Row(new { Code = "153001966", Name = "VaBank" })
                .Row(new { Code = "153001795", Name = new ExplicitUnicodeString("ОАО \"АСБ Беларусбанк\"") })
                .Row(new { Code = "153001110", Name = new ExplicitUnicodeString("ЗАО \"РРБ Банк\"") })
                //.Row(new { Code = "153001175", Name = new ExplicitUnicodeString("ЗАО \"БелСвиссБанк\"") })

                .Row(new { Code = "153001108", Name = new ExplicitUnicodeString("ЗАО БАНК ВТБ (БЕЛАРУСЬ)") })
                .Row(new { Code = "153001111", Name = new ExplicitUnicodeString("МЕЖГОСУДАРСТВЕННЫЙ БАНК") })
                .Row(new { Code = "153001117", Name = new ExplicitUnicodeString("ЗАО 'МТБАНК'") })
                .Row(new { Code = "153001141", Name = new ExplicitUnicodeString("ОАО 'ХКБАНК'") })
                .Row(new { Code = "153001175", Name = new ExplicitUnicodeString("ЗАО 'БСБ БАНК'") })
                .Row(new { Code = "153001182", Name = new ExplicitUnicodeString("ОАО 'ТЕХНОБАНК'") })
                .Row(new { Code = "153001222", Name = new ExplicitUnicodeString("ОАО 'БАНК РАЗВИТИЯ РЕСПУБЛИКИ БЕЛАРУСЬ'") })
                .Row(new { Code = "153001226", Name = new ExplicitUnicodeString("ОАО 'БАНК БЕЛВЭБ'") })
                .Row(new { Code = "153001266", Name = new ExplicitUnicodeString("ОАО 'ФРАНСАБАНК'") })
                .Row(new { Code = "153001270", Name = new ExplicitUnicodeString("ЗАО 'АЛЬФА-БАНК'") })
                .Row(new { Code = "153001272", Name = new ExplicitUnicodeString("ОАО 'БАНК МОСКВА-МИНСК'") })
                .Row(new { Code = "153001273", Name = new ExplicitUnicodeString("ЗАО 'ИНТЕРПЭЙБАНК'") })
                .Row(new { Code = "153001281", Name = new ExplicitUnicodeString("ЗАО 'ДЕЛЬТА БАНК'") })
                .Row(new { Code = "153001288", Name = new ExplicitUnicodeString("ЗАО 'ТРАСТБАНК'") })
                .Row(new { Code = "153001333", Name = new ExplicitUnicodeString("ЗАО 'ТК БАНК'") })
                .Row(new { Code = "153001369", Name = new ExplicitUnicodeString("ОАО 'БПС-СБЕРБАНК'") })
                .Row(new { Code = "153001704", Name = new ExplicitUnicodeString("ЗАО 'БТА БАНК'") })
                .Row(new { Code = "153001735", Name = new ExplicitUnicodeString("ОАО 'ЕВРОТОРГИНВЕСТБАНК'") })
                .Row(new { Code = "153001739", Name = new ExplicitUnicodeString("ОАО 'БЕЛИНВЕСТБАНК'") })
                .Row(new { Code = "153001742", Name = new ExplicitUnicodeString("ОАО 'БЕЛГАЗПРОМБАНК'") })
                .Row(new { Code = "153001749", Name = new ExplicitUnicodeString("ОАО 'ПРИОРБАНК'") })
                .Row(new { Code = "153001755", Name = new ExplicitUnicodeString("ЗАО 'ИДЕЯ БАНК'") })
                .Row(new { Code = "153001765", Name = new ExplicitUnicodeString("ОАО 'БНБ-БАНК'") });
        }

        public override void Down()
        {
        }
    }
}
