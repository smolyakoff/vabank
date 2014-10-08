namespace VaBank.Common.Data.Paging
{
    public interface IClientPageable : IClientQuery
    {
        ClientPage ClientPage { get; set; }
    }
}
