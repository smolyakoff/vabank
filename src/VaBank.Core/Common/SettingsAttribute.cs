using System;

namespace VaBank.Core.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SettingsAttribute : Attribute
    {
        private readonly string _key;

        public SettingsAttribute()
        {
            _key = null;
        }

        public SettingsAttribute(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key should not be empty", "key");
            }
            _key = key;
        }

        public string GetKey(Type settingsType)
        {
            if (settingsType == null)
            {
                throw new ArgumentNullException("settingsType");
            }
            return _key ?? settingsType.FullName;
        }
    }
}
