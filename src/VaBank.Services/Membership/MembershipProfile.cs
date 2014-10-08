using AutoMapper;
using VaBank.Core.Membership;
using VaBank.Services.Contracts.Membership;

namespace VaBank.Services.Membership
{
    internal class MembershipProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ApplicationToken, TokenModel>();
            CreateMap<ApplicationClient, ApplicationClientModel>();
        }
    }
}
