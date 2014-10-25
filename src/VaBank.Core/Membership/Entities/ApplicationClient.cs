using VaBank.Core.Common;

namespace VaBank.Core.Membership.Entities
{
    public class ApplicationClient : Entity<string>
    {
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public ApplicationClientType ApplicationType { get; set; }
        public string AllowedOrigin { get; set; }
        public string Secret { get; set; }
    }
}