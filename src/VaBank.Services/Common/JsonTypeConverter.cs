using AutoMapper;
using Newtonsoft.Json.Linq;

namespace VaBank.Services.Common
{
    public class JsonTypeConverter : ITypeConverter<string, JObject>
    {
        public JObject Convert(ResolutionContext context)
        {
            var source = context.SourceValue as string;
            return source == null ? null : JObject.Parse(source);
        }
    }
}
