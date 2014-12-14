using VaBank.Services.Contracts.Common.Models;

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
