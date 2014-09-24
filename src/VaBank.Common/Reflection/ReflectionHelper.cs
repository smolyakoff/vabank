using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Reflection
{
    public static class ReflectionHelper
    {
        public static PropertyInfo FindProperty(this Type type, string name, StringComparison comparison)
        {
            foreach (var property in type.GetProperties())
            {
                if (property.Name.Equals(name, comparison))
                    return property;
            }
            return null;
        }

        public static PropertyInfo FindProperty<T>(string name, StringComparison comparison)
        {
            return FindProperty(typeof(T), name, comparison);
        }
    }
}
