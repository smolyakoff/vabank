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
            CreateMap<CreateTokenCommand, TokenModel>();
            CreateMap<ApplicationClient, ApplicationClientModel>();
            CreateMap<UserClaim, ClaimModel>();
            CreateMap<User, UserIdentityModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(y => y.Id));
        }
    }
}
