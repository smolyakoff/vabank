using System;
using VaBank.Common.Validation;

namespace VaBank.Common.Data.Repositories
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IncludeAttribute : Attribute
    {
        private readonly string[] _includedProperties;

        public IncludeAttribute(params string[] includedProperties)
        {
            Argument.NotNull(includedProperties, "propertyInclusion");
            Argument.Satisfies(includedProperties, x => x.Length > 0, "propertyInclusion",
                "Included properties length should be greater than zero.");
            _includedProperties = includedProperties;
        }

        public string[] IncludedProperies { get { return _includedProperties; } }
    }
}
