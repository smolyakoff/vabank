using VaBank.Core.App.Entities;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting.Entities
{
    public class CardVendor: Entity<string>
    {
        protected CardVendor()
        {
        }

        public string Name { get; protected set; }

        public virtual FileLink Image { get; protected set; }
    }
}
