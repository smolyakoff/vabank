using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Maintenance.Models;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class BankOwnerModel : IOwnerModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Id
        {
            get { return Code; }
        }
    }
}
