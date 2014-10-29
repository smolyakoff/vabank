namespace VaBank.Common.Data
{
    public class IdentityQuery<T> : IIdentityQuery<T>
    {
        public IdentityQuery()
        {
        } 

        public IdentityQuery(T id)
        {
            Id = id;
        }

        public T Id { get; set; }
    }
}
