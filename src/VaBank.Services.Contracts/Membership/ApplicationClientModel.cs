namespace VaBank.Services.Contracts.Membership
{
    public class ApplicationClientModel
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifetime { get; set; }
        public ApplicationClientTypeModel ApplicationType { get; set; }
        public string AllowedOrigin { get; set; }
        public string Secrete { get; set; }
    }
}
