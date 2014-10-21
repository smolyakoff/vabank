using VaBank.Services.Contracts.Common;

namespace VaBank.Services.Common
{
    internal static class NotFound
    {
        public static DataNotFoundException Exception()
        {
            return new DataNotFoundException();
        }

        public static DataNotFoundException ExceptionFor<T>(object key)
        {
            return new DataNotFoundException(typeof(T), key);
        } 
    }
}
