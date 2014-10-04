using System;
using AutoMapper;
using VaBank.Core.Maintenance;
using VaBank.Services.Contracts.Admin.Maintenance;

namespace VaBank.Services.Maintenance
{
    internal class MaintenanceProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<SystemLogEntry, SystemLogEntryBriefModel>()
                .ForMember(dest => dest.EventId, src => src.MapFrom(x => x.Id));
            CreateMap<SystemLogEntry, SystemLogExceptionModel>()
                .ForMember(dest => dest.EventId, src => src.MapFrom(x => x.Id));
            CreateMap<SystemLogEntry, SystemLogTypeModel>();
        }
    }
}
