namespace VaBank.Common.Data
{
    public interface IIdentityQuery<out T> : IQuery
    {
        T Id { get; }
    }
}
