using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Membership.Entities;

namespace VaBank.Services.Common
{
    public class VaBankIdentity : ClaimsIdentity
    {
        private readonly IRepository<User> _userRepository;

        public VaBankIdentity(IRepository<User> userRepository)
            :this (Thread.CurrentPrincipal.Identity as ClaimsIdentity, userRepository)
        {
        }

        private VaBankIdentity(ClaimsIdentity threadIdentity, IRepository<User> userRepository) : base(threadIdentity)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }
            _userRepository = userRepository;
            if (threadIdentity != null)
            {
                var idClaim = threadIdentity.FindFirst(UserClaim.Types.UserId);
                if (idClaim != null)
                {
                    UserId = Guid.Parse(idClaim.Value);
                }
                var clientClaim = threadIdentity.FindFirst(UserClaim.Types.ClientId);
                if (clientClaim != null)
                {
                    ClientApplicationId = clientClaim.Value;
                }
            }
        }

        public Guid? UserId { get; private set; }

        public string ClientApplicationId { get; private set; }

        public User User
        {
            get { return UserId == null ? null : _userRepository.Find(UserId.Value); }
        }

        public bool IsInRole(string roleName)
        {
            var roles = FindAll(RoleClaimType);
            return roles.Any(x => x.Value == roleName);
        }
    }
}
