namespace VaBank.Core.Processing
{
    public interface ICurrencyConverter
    {
        decimal ConvertFromBase(decimal amount);
        decimal ConvertFromForeign(decimal amount);
    }
}