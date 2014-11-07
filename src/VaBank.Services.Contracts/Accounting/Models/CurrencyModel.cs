namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CurrencyModel
    {
        public string ISOName { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }

        public int Precision { get; set; }
    }
}
