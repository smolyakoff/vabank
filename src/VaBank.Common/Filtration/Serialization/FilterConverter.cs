using System;
using System.ComponentModel;
using System.Globalization;

namespace VaBank.Common.Filtration.Serialization
{
    //converter from string: useful for get requests
    public class FilterConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue))
            {
                return base.ConvertFrom(context, culture, value);
            }
            return Parse(stringValue);
        }

        private Filter Parse(string filterString)
        {
            return null;
        }
    }
}
