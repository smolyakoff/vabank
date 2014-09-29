using AutoMapper;
using VaBank.Core.Entities;
using VaBank.Services.Contracts.Admin.Maintenance;

namespace VaBank.Services.Admin.Maintenance
{
    internal class MaintenanceProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Log, SystemLogEntryModel>();
        }
    }
}
