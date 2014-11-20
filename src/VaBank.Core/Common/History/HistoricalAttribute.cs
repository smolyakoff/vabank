using System;
using VaBank.Common.Validation;

namespace VaBank.Core.Common.History
{
    public class HistoricalAttribute : Attribute
    {
        private readonly object _specification;

        public HistoricalAttribute(Type historicalEntitySpecification)
        {
            Argument.NotNull(historicalEntitySpecification, "historicalEntitySpecification");

            _specification = Activator.CreateInstance(historicalEntitySpecification);
        }

        public IHistoricalEntitySpecification<T> Spec<T>()
        {
            return (IHistoricalEntitySpecification<T>) _specification;
        } 
    }
}
