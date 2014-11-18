using AutoMapper;
using VaBank.Core.App.Entities;
using VaBank.Core.Maintenance.Entitities;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Contracts.Maintenance.Commands;
using VaBank.Services.Contracts.Maintenance.Models;

namespace VaBank.Services.Maintenance
{
    internal class MaintenanceProfile : Profile
    {
        protected override void Configure()
        {
            //System Log
            CreateMap<SystemLogEntry, SystemLogEntryBriefModel>()
                .ForMember(dest => dest.EventId, src => src.MapFrom(x => x.Id));
            CreateMap<SystemLogEntry, SystemLogExceptionModel>()
                .ForMember(dest => dest.EventId, src => src.MapFrom(x => x.Id));
            CreateMap<SystemLogEntry, SystemLogTypeModel>();

            //Audit Log
            CreateMap<ApplicationAction, AppActionModel>()
                .ForMember(dest => dest.JsonData, src => src.MapFrom(x => x.Data));
            CreateMap<AuditLogBriefEntry, AuditLogEntryBriefModel>()
                .ForMember(dest => dest.OperationId, src => src.MapFrom(x => x.Operation.Id))
                .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.Operation.UserId))
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.Operation.User.UserName))
                .ForMember(dest => dest.ApplicationId, src => src.MapFrom(x => x.Operation.ClientApplicationId))
                .ForMember(dest => dest.StartedUtc, src => src.MapFrom(x => x.Operation.StartedUtc))
                .ForMember(dest => dest.AppActions, src => src.MapFrom(x => x.ApplicationActions));

            CreateMap<DatabaseOperation, DatabaseOperationModel>();
            CreateMap<VersionedDatabaseRow, DbChangeModel>()
                .ForMember(des => des.Operation, src => src.MapFrom(x => x.DatabaseOperation))
                .ForMember(des => des.Values, src => src.MapFrom(x => x.Values));
            CreateMap<DatabaseAction, DbActionModel>()
                .ForMember(dest => dest.Changes, src => src.MapFrom(x => x.Rows));
            CreateMap<AuditLogEntry, AuditLogEntryModel>()
                .ForMember(des => des.DbActions, src => src.MapFrom(x => x.DatabaseActions));

            CreateMap<LogAppActionCommand, ApplicationAction>();

            CreateMap<ITransaction, TransactionLogEntryBriefModel>()
                .ForMember(x => x.TransactionId, cfg => cfg.MapFrom(x => x.Id))
                .Include<Transaction, TransactionLogEntryBriefModel>()
                .Include<HistoricalTransaction, TransactionLogEntryHistoricalModel>();
            CreateMap<Transaction, TransactionLogEntryBriefModel>();
            CreateMap<HistoricalTransaction, TransactionLogEntryHistoricalModel>()
                .ForMember(x => x.TimestampUtc, cfg => cfg.MapFrom(x => x.HistoryTimestampUtc))
                .ForMember(x => x.ChangeOwnerUserId, cfg => cfg.MapFrom(x => x.HistoryOperation.UserId));
        }
    }
}
