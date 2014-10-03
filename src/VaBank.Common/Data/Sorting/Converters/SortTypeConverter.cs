using System;
using System.ComponentModel;
using System.Globalization;
using VaBank.Common.Data.Filtering;

namespace VaBank.Common.Data.Sorting.Converters
{
    public class SortTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            try
            {
                var stringValue = value as string;
                if (string.IsNullOrEmpty(stringValue))
                {
                    return new EmptyFilter();
                }
                return new DynamicLinqFilter(stringValue);
            }
            catch (Exception)
            {
                return new EmptySort();
            }
        }
    }
}
