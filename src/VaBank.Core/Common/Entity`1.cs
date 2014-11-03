namespace VaBank.Core.Common
{
    public abstract class Entity<TId> : Entity
    {
        public TId Id { get; internal set; }
    }
}