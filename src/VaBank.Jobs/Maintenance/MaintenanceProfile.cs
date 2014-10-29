using AutoMapper;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Maintenance.Commands;

namespace VaBank.Jobs.Maintenance
{
    public class MaintenanceProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<IAuditedEvent, LogAppActionCommand>();
        }
    }
}
