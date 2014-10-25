using System;

namespace VaBank.Common.IoC
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InjectableAttribute : Attribute
    {
        public InjectableAttribute()
        {
            Lifetime = Lifetime.PerDependency;
        }

        public Lifetime Lifetime { get; set; }
    }
}
