namespace VaBank.Common.Data.Filtering
{
    public interface IClientFilterableQuery : IFilterableQuery
    {
        void ApplyFilter(IFilter filter);
    }
}
