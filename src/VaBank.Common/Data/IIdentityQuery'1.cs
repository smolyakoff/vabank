namespace VaBank.Common.Data
{
    public interface IIdentityQuery<out T> : IClientQuery
    {
        T Id { get; }
    }
}
