using System;
using System.Globalization;
using System.Linq;
using VaBank.Common.Validation;

namespace VaBank.Common.Util
{
    public static class Randomizer
    {
        private static readonly Random Random = new Random();

        public static T Choose<T>(params T[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Length == 1)
            {
                throw new ArgumentException("Items array should contain at least one element.", "items");
            }
            var index = Random.Next(0, items.Length);
            return items[index];
        }

        public static string NumericString(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "Length should be 0 or greater");
            }
            var numbers = Enumerable.Range(0, length)
                .Select(x => Random.Next(0, 10))
                .Select(x => x.ToString(CultureInfo.InvariantCulture));
            return string.Join(string.Empty, numbers.ToArray());
        }

        public static double FromRange(Range<double> range)
        {
            Argument.NotNull(range, "range");
            return range.LowerBound + (range.UpperBound - range.LowerBound) * Random.NextDouble();
        }
    }
}
