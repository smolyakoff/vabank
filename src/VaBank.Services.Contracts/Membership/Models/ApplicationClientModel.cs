namespace VaBank.Services.Contracts.Membership.Models
{
    public class ApplicationClientModel
    {
        public string Id { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifetime { get; set; }
        public ApplicationClientTypeModel ApplicationType { get; set; }
        public string AllowedOrigin { get; set; }
        public string Secret { get; set; }
    }
}
