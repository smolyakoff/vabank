namespace VaBank.Core.Common
{
    public interface IVersionedEntity
    {
        byte[] RowVersion { get; }
    }
}
