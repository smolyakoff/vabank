using System;
using System.Collections.Generic;

namespace VaBank.Services.Contracts.Membership.Models
{
    public class UserIdentityModel : IUserModel
    {
        public UserIdentityModel()
        {
            Claims = new List<ClaimModel>();
        }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public List<ClaimModel> Claims { get; set; } 
    }
}
