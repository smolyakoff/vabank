namespace VaBank.Services.Contracts.Payments.Queries
{
    public interface IRangeQuery<out T>
    {
        T From { get; }

        T To { get; }
    }
}