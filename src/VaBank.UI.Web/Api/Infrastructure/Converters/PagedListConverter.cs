using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using PagedList;

namespace VaBank.UI.Web.Api.Infrastructure.Converters
{
    public class PagedListConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var list = value as IPagedList;
            if (list == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            serializer.Serialize(writer, new
            {
                items = ((IEnumerable)list).Cast<object>().ToArray(),
                pageNumber = list.PageNumber,
                pageSize = list.PageSize,
                totalItems = list.TotalItemCount,
                totalPages = list.PageCount,
                skip = (list.PageNumber - 1) * list.PageSize
            });
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (IPagedList).IsAssignableFrom(objectType);
        }
    }
}