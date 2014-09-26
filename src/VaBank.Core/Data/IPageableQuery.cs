namespace VaBank.Core.Data
{
    public interface IPageableQuery<T> : ISortableQuery<T>
        where T : class
    {
        int PageSize { get; }

        int PageNumber { get; }
    }
}
