using VaBank.Common.Data.Sorting;

namespace VaBank.Common.Data.Paging
{
    public interface IClientPageable : IClientSortable
    {
        ClientPage ClientPage { get; set; }
    }
}
