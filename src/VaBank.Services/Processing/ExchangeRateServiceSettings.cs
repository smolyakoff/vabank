namespace VaBank.Services.Processing
{
    public class ExchangeRateServiceSettings
    {
        public ExchangeRateServiceSettings()
        {
            NationalCurrencyISOName = "BYR";
        }

        public string NationalCurrencyISOName { get; private set; }
    }
}
