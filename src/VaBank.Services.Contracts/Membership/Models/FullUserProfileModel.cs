namespace VaBank.Services.Contracts.Membership.Models
{
    public class FullUserProfileModel
    {
        public UserBriefModel User { get; set; }

        public UserProfileModel Profile { get; set; }

        public UserPaymentProfileModel PaymentProfile { get; set; }
    }
}
