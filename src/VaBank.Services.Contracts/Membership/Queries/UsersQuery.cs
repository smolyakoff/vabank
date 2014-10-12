using System.Linq;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Paging;
using VaBank.Common.Data.Sorting;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Contracts.Membership.Queries
{
    public class UsersQuery : IClientPageable, IClientFilterable
    {
        public UsersQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
            ClientPage = new ClientPage {PageNumber = 1, PageSize = 15};
            ClientSort = new DelegateSort<UserBriefModel>(q => q.OrderBy(m => m.UserName));
            Roles = new string[]{};
        }

        public string[] Roles { get; set; }

        public ClientPage ClientPage { get; set; }
        public IFilter ClientFilter { get; set; }
        public ISort ClientSort { get; set; }
    }
}
