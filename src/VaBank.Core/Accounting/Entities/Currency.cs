using VaBank.Core.Common;

namespace VaBank.Core.Accounting.Entities
{
    public class Currency: Entity
    {
        protected Currency()
        {
        }

        public string ISOName { get; protected set; }
        public string Symbol { get; protected set; }
        public string Name { get; protected set; }
        public int Precision { get; protected set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", ISOName, Symbol);
        }
    }
}
