using AutoMapper;
using VaBank.Core.Maintenance;
using VaBank.Services.Contracts.Maintenance.Models;
using VaBank.Core.App;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Maintenance.Commands;

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

            CreateMap<ApplicationAction, AppActionModel>()
                .ForMember(dest => dest.ActionId, src => src.MapFrom(x => x.EventId))
                .ForMember(dest => dest.JsonData, src => src.MapFrom(x => x.Data));
            CreateMap<AuditLogBriefEntry, AuditLogEntryBriefModel>()
                .ForMember(des => des.OperationId, src => src.MapFrom(x => x.Operation.Id))
                .ForMember(des => des.UserId, src => src.MapFrom(x => x.Operation.UserId))
                .ForMember(des => des.AppActions, src => src.MapFrom(x => x.ApplicationActions));

            CreateMap<DatabaseOperation, DatabaseOperationModel>();
            CreateMap<VersionedDatabaseRow, DbChangeModel>()
                .ForMember(des => des.Action, src => src.MapFrom(x => x.Action))
                .ForMember(des => des.Values, src => src.MapFrom(x => x.Values));
            CreateMap<DatabaseAction, DbActionModel>();
            CreateMap<AuditLogEntry, AuditLogEntryModel>()
                .ForMember(des => des.DbActions, src => src.MapFrom(x => x.DatabaseActions));

            CreateMap<IAuditedEvent, LogAppActionCommand>();
            CreateMap<LogAppActionCommand, ApplicationAction>();
        }
    }
}
