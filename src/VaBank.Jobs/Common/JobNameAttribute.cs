using System;

namespace VaBank.Jobs.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class JobNameAttribute : Attribute
    {
        public JobNameAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            Name = name;
        }

        public string Name { get; private set; }
    }
}
