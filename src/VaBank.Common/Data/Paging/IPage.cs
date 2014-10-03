namespace VaBank.Common.Data.Paging
{
    public interface IPage
    {
        int PageNumber { get; }

        int PageSize { get; }
    }
}