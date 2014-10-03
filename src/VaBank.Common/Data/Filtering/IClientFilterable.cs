namespace VaBank.Common.Data.Filtering
{
    public interface IClientFilterable
    {
        void ApplyFilter(IFilter filter);
    }
}