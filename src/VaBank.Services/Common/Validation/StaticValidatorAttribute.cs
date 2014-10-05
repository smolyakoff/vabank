using System;

namespace VaBank.Services.Common.Validation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StaticValidatorAttribute : Attribute
    {
    }
}
