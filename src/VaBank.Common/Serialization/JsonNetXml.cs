using Newtonsoft.Json;

namespace VaBank.Common.Serialization
{
    public class JsonNetXml
    {
        public static string SerializeObject<T>(T value, 
            string rootElementName = "Root",
            JsonSerializerSettings settings = null)
        {
            settings = settings ??
                       (JsonConvert.DefaultSettings == null
                           ? new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Arrays}
                           : JsonConvert.DefaultSettings());
            var json = JsonConvert.SerializeObject(value, settings);
            var xmlElement = JsonConvert.DeserializeXNode(json, rootElementName);
            return xmlElement.ToString();
        }
    }
}
