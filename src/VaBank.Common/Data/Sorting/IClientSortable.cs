namespace VaBank.Common.Data.Sorting
{
    public interface IClientSortable : IClientQuery
    {
        ISort ClientSort { get; set; }
    }
}