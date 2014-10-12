using System;
using Newtonsoft.Json.Linq;
using VaBank.Services.Common.Validation;

namespace VaBank.UI.Web.Api.Infrastructure.IoC
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