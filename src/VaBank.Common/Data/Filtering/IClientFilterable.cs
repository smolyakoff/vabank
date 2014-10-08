namespace VaBank.Common.Data.Filtering
{
    public interface IClientFilterable : IClientQuery
    {
        IFilter ClientFilter { get; set; }
    }
}