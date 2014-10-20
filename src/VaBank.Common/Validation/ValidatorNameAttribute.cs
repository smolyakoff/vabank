using System;

namespace VaBank.Common.Validation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ValidatorNameAttribute : Attribute
    {
        public ValidatorNameAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Name should not be empty");
            }
            Name = name;
        }

        public string Name { get; private set; }
    }
}
