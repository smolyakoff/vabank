using System;
using Newtonsoft.Json.Linq;

namespace VaBank.Common.Data
{
    public class JsonNetConverter : IObjectConverter
    {
        public object Convert(object obj, Type destinationType)
        {
            if (obj == null)
            {
                return null;
            }
            var jObject = obj as JToken;
            return jObject == null ? obj : jObject.ToObject(destinationType);
        }
    }
}