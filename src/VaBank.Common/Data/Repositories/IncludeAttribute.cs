using System;
using VaBank.Common.Validation;

namespace VaBank.Common.Data.Repositories
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IncludeAttribute : Attribute
    {
        private readonly string[] _included;

        public IncludeAttribute(params string[] included)
        {
            Argument.NotNull(included, "included");
            Argument.Satisfies(included, x => x.Length > 0, "included", 
                "Included properties length should be greater than zero.");
            _included = included;
        }

        public string[] Included
        {
            get { return _included; }
        }
    }
}
