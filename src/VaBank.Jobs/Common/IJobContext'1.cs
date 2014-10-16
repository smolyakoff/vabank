namespace VaBank.Jobs.Common
{
    public interface IJobContext<T> : IJobContext
    {
        T Data { get; set; }
    }
}
