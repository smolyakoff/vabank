namespace VaBank.Services.Contracts.Common.Queries
{
    public interface IRangeQuery<out T>
    {
        T From { get; }

        T To { get; }
    }
}