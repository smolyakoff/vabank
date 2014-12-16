using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using VaBank.Common.Data.Linq;
using VaBank.Common.Security;
using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Core.Membership.Entities
{
    public class User : Entity<Guid>, IVersionedEntity
    {
        public static User Create(string userName, string roleName, string password)
        {
            Argument.EnsureIsValid<UserNameValidator, string>(userName, "userName");
            Argument.EnsureIsValid<RoleValidator, string>(roleName, "roleName");
            Argument.EnsureIsValid<PasswordValidator, string>(password, "password");
            var user = new User {Id = Guid.NewGuid(), UserName = userName};
            user.Claims.Add(new UserClaim {UserId = user.Id, Type = ClaimTypes.Sid, Value = user.Id.ToString()});
            user.Claims.Add(new UserClaim {UserId = user.Id, Type = ClaimTypes.Name, Value = userName});
            user.Claims.Add(new UserClaim {UserId = user.Id, Type = ClaimTypes.Role, Value = roleName});
            user.UpdatePassword(password);
            return user;
        }

        public static class Spec
        {
            public static LinqSpec<User> Active = LinqSpec.For<User>(x => !x.Deleted);
        }

        protected User()
        {
            Claims = new Collection<UserClaim>();
        }

        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }
        public bool LockoutEnabled { get; internal set; }
        public DateTime? LockoutEndDateUtc { get; internal set; }
        public string UserName { get; set; }
        public int AccessFailedCount { get; internal set; }
        public bool Deleted { get; set; }
        public virtual ICollection<UserClaim> Claims { get; private set; }
        public virtual UserProfile Profile { get; set; }
        public byte[] RowVersion { get; set; }

        public bool IsActive
        {
            get { return !Deleted; }
        }

        public bool ValidatePassword(string passwordText)
        {
            return !string.IsNullOrEmpty(passwordText) && Password.Validate(PasswordHash, PasswordSalt, passwordText);
        }

        public void UpdatePassword(string passwordText)
        {
            Argument.EnsureIsValid<PasswordValidator, string>(passwordText, "passwordText");
            var password = Password.Create(passwordText);
            PasswordHash = password.PasswordHash;
            PasswordSalt = password.PasswordSalt;
        }
    }
}