using FluentMigrator;

namespace VaBank.Data.Migrations
{
    internal class M2Currency
    {
        public string CurrencyISOName { get; set; }

        public ExplicitUnicodeString Name { get; set; }

        public string Symbol { get; set; }

        public static M2Currency Create(string isoName, string name, string symbol)
        {
            var currency = new M2Currency
            {
                CurrencyISOName = isoName,
                Name = new ExplicitUnicodeString(name),
                Symbol = symbol
            };
            return currency;
        }
    }
}
