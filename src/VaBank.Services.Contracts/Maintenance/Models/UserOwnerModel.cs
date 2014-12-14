using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class UserOwnerModel : UserNameModel, IOwnerModel
    {
        public string Name
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public string Id
        {
            get { return UserId.ToString(); }
        }
    }
}
