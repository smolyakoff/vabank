using Newtonsoft.Json;

namespace VaBank.Common.Serialization
{
    public class JsonNetXml
    {
        public static string SerializeObject<T>(T value, 
            string rootElementName = "Root",
            JsonSerializerSettings settings = null)
        {
            var json = JsonConvert.SerializeObject(value, settings ?? JsonConvert.DefaultSettings());
            var xmlElement = JsonConvert.DeserializeXNode(json, rootElementName);
            return xmlElement.ToString();
        }
    }
}
