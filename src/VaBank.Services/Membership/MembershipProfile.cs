using AutoMapper;
using VaBank.Core.Membership;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Membership
{
    internal class MembershipProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ApplicationToken, TokenModel>();
            CreateMap<CreateTokenCommand, ApplicationToken>();
            CreateMap<ApplicationClient, ApplicationClientModel>();
            CreateMap<UserClaim, ClaimModel>();
            CreateMap<User, UserIdentityModel>();
        }
    }
}
