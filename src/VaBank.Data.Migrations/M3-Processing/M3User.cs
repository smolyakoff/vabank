using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VaBank.Common.Security;

namespace VaBank.Data.Migrations
{
    internal class M3User
    {
        public static M3User Create(string login, string password, string role, M1Profile profile = null)
        {
            var user = new M3User
            {
                UserId = Guid.NewGuid(),
                UserName = login,
                Password = Password.Create(password),
                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login),
                    new Claim(ClaimTypes.Role, role)
                },
                Profile = profile
            };
            user.Claims.Add(new Claim(ClaimTypes.Sid, user.UserId.ToString()));
            var credentials = user.Claims.OrderBy(x => x.Type).Select(x => x.ToString());
            return user;
        }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public Password Password { get; set; }

        public IList<Claim> Claims { get; set; }

        public M1Profile Profile { get; set; }

        public object ToTableRow()
        {
            return new
            {
                UserId,
                UserName,
                Password.PasswordHash,
                Password.PasswordSalt,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                Deleted = false,
            };
        }
    }
}
