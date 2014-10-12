using System;
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
            CreateMap<User, UserBriefModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(y => y.Id))
                .ForMember(x => x.Email, cfg => cfg.MapFrom(y => y.Profile == null ? null : y.Profile.Email))
                .ForMember(x => x.FirstName, cfg => cfg.MapFrom(y => y.Profile == null ? null : y.Profile.FirstName))
                .ForMember(x => x.LastName, cfg => cfg.MapFrom(y => y.Profile == null ? null : y.Profile.LastName));
            CreateMap<UserProfile, UserProfileModel>();
            CreateMap<CreateUserCommand, UserProfile>()
                .ForMember(x => x.PhoneNumber, cfg => cfg.MapFrom(x => string.IsNullOrWhiteSpace(x.PhoneNumber) ? null : x.PhoneNumber));
            CreateMap<CreateUserCommand, User>()
                .ConstructUsing(c =>
                {
                    var user = User.Create(c.UserName, c.Role, c.Password);
                    user.Profile = new UserProfile(user.Id);
                    return user;
                })
                .ForMember(x => x.Profile, cfg => cfg.MapFrom(x => x));
        }
    }
}
