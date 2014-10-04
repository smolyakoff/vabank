using VaBank.Common.Data;

namespace VaBank.Services.Contracts.Common.Queries
{
    public class IdentityQuery<T> : IIdentityQuery<T>
    {
        public T Id { get; set; }
    }
}
