namespace VaBank.Jobs.Common
{
    public class DefaultJobContext<T> : DefaultJobContext, IJobContext<T>
    {
        public T Data { get; set; }
    }
}
