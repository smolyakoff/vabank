using Newtonsoft.Json;

namespace VaBank.Jobs.Common
{
    internal static class Serialization
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }
}
