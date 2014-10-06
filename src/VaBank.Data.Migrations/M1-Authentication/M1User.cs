using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VaBank.Common.Security;

namespace VaBank.Data.Migrations
{
    internal class M1User
    {
        public static M1User Create(string login, string password, string role)
        {
            var user = new M1User
            {
                UserId = Guid.NewGuid(),
                UserName = login,
                Password = Password.Create(password),
                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, login),
                    new Claim(ClaimTypes.Role, role)
                }
            };
            var credentials = new List<string>
            {
                login,
                user.Password.PasswordHash,
            };
            credentials.AddRange(user.Claims.OrderBy(x => x.Type).Select(x => x.ToString()));
            user.SecurityStamp = Hash.Compute(credentials);
            return user;
        }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public Password Password { get; set; }

        public string SecurityStamp { get; set; }

        public IList<Claim> Claims { get; set; }

        public object ToTableRow()
        {
            return new
            {
                UserId,
                UserName,
                Password.PasswordHash,
                Password.PasswordSalt,
                SecurityStamp,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                Deleted = false,
            };
        }
    }
}
