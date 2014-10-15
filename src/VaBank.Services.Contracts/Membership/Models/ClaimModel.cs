using System.Security.Claims;

namespace VaBank.Services.Contracts.Membership.Models
{
    public class ClaimModel
    {
        public static class Types
        {
            public const string Role = ClaimTypes.Role;

            public const string UserId = ClaimTypes.Sid;

            public const string UserName = ClaimTypes.Name;

            public const string ClientId = "https://vabank.azurewebsites.net/api/claimTypes/clientId";
        }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}
