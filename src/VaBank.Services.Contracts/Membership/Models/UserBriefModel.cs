namespace VaBank.Services.Contracts.Membership.Models
{
    public class UserBriefModel : UserIdentityModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool Deleted { get; set; }

        public int AccessFailedCount { get; set; }
    }
}
