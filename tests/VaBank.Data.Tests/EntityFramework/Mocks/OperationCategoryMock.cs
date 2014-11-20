using VaBank.Core.Processing.Entities;

namespace VaBank.Data.Tests.EntityFramework.Mocks
{
    internal class OperationCategoryMock : OperationCategory
    {
        public OperationCategoryMock(string code, string name, string description)
        {
            Code = code;
            Name = name;
            Description = description;
        }
    }
}
