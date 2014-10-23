using System;

namespace VaBank.Services.Contracts.Common.Models
{
    public class UserNameModel : IUserModel
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
