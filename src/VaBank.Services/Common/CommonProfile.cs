using System;
using AutoMapper;
using Newtonsoft.Json.Linq;
using VaBank.Common.Resources;
using VaBank.Core.App.Entities;
using VaBank.Core.Membership.Entities;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Common
{
    public class CommonProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<string, JObject>().ConvertUsing<JsonTypeConverter>();
            CreateMap<FileLink, Link>();

            CreateMap<User, UserNameModel>()
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.Email, cfg => cfg.MapFrom(y => y.Profile == null ? null : y.Profile.Email))
                .ForMember(x => x.FirstName, cfg => cfg.MapFrom(y => y.Profile == null ? null : y.Profile.FirstName))
                .ForMember(x => x.LastName, cfg => cfg.MapFrom(y => y.Profile == null ? null : y.Profile.LastName));
        }
    }
}
