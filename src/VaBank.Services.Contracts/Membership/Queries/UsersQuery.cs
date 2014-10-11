using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Paging;

namespace VaBank.Services.Contracts.Membership.Queries
{
    public class UsersQuery : IClientPageable, IClientFilterable
    {
        public UsersQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
            ClientPage = new ClientPage {PageNumber = 1, PageSize = 15};
        }

        public ClientPage ClientPage { get; set; }
        public IFilter ClientFilter { get; set; }
    }
}
