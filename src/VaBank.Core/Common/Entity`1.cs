namespace VaBank.Core.Common
{
    public class Entity<TId> : Entity
    {
        public TId Id { get; set; }
    }
}