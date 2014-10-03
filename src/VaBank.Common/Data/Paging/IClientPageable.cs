namespace VaBank.Common.Data.Paging
{
    public interface IClientPageable
    {
        void ApplyPaging(int? pageNumber, int? pageSize);
    }
}
