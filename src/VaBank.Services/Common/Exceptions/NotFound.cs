using VaBank.Services.Contracts.Common;

namespace VaBank.Services.Common.Exceptions
{
    internal static class NotFound
    {
        public static DataNotFoundException Exception()
        {
            return new DataNotFoundException();
        }

        public static DataNotFoundException ExceptionFor<T>()
        {
            return new DataNotFoundException(typeof(T));
        }

        public static DataNotFoundException ExceptionFor<T>(object key)
        {
            return new DataNotFoundException(typeof(T), key);
        }

        public static DataNotFoundException ExceptionFor<T>(params object[] keys)
        {
            if (keys.Length == 1)
            {
                return ExceptionFor<T>(keys[0]);
            }
            var keysString = string.Join(",", keys);
            return new DataNotFoundException(typeof(T), keysString);
        } 
    }
}
