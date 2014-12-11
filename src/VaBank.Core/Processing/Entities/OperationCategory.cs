using System.Collections.Generic;
using VaBank.Core.Common;

namespace VaBank.Core.Processing.Entities
{
    public class OperationCategory : Entity, IReferenceEntity
    {
        protected OperationCategory()
        {
            Children = new List<OperationCategory>();
        }

        public string Code { get; protected set; }

        public string Name { get; protected set; }

        public string Description { get; protected set; }

        public virtual OperationCategory Parent { get; set; }

        public virtual ICollection<OperationCategory> Children { get; set; }
    }
}
